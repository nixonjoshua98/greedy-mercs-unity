using SRC.Mercs.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.Mercs.Data
{
    public class MercDataContainer : Core.GMClass
    {
        private readonly Dictionary<MercID, MercLocalDataFileMerc> LocalDataFiles = new();
        private readonly Dictionary<MercID, AggregatedMercData> MercData = new();
        private readonly Dictionary<MercID, MercGameData> MercGameDataLookup = new();
        private readonly Dictionary<MercID, MercUserData> MercUserDataLookup = new();

        private List<MercLocalUserData> LocalStates => App.LocalStateFile.MercStates;

        public UnityEvent<MercID> E_OnMercUnlocked { get; set; } = new UnityEvent<MercID>();

        /// <summary>
        /// Update all stored static and user data
        /// </summary>
        public void Set(List<MercUserData> userMercs, MercDataFile staticData)
        {
            if (LocalDataFiles.Count == 0)
            {
                LoadLocalData().ToList().ForEach(kv => LocalDataFiles[kv.Key] = kv.Value);
            }

            UpdateLocalStateFile(userMercs);
            UpdateStoredGameData(staticData);
        }

        private void UpdateLocalStateFile(List<MercUserData> mercs)
        {
            // Update stored user data
            MercUserDataLookup.Clear();
            mercs.ForEach(m => MercUserDataLookup[m.ID] = m);

            Dictionary<MercID, MercLocalUserData> states = new();

            // Update local states
            mercs.ForEach(merc =>
            {
                var savedState = LocalStates.FirstOrDefault(x => x.ID == merc.ID);

                states[merc.ID] = savedState ?? new MercLocalUserData(merc.ID);
            });

            App.LocalStateFile.MercStates = states.Values.ToList();
        }

        /// <summary>
        /// Add newly unlocked merc to the data we have (invokes event)
        /// </summary>
        public void AddNewUnlockedMerc(MercID mercId)
        {
            LocalStates.Add(new MercLocalUserData(mercId));

            E_OnMercUnlocked.Invoke(mercId);
        }

        /// <summary>
        /// Update the internal static game data we have
        /// </summary>
        private void UpdateStoredGameData(MercDataFile model)
        {
            // Update stored game data lookup
            MercGameDataLookup.Clear();
            model.Mercs.ForEach(m => MercGameDataLookup[m.MercID] = m);


            Dictionary<int, MercPassiveBonus> passives = model.Passives.ToDictionary(x => x.PassiveID, x => x);

            for (int i = 0; i < model.Mercs.Count; i++)
            {
                MercGameData merc = model.Mercs[i];

                if (!LocalDataFiles.TryGetValue(merc.MercID, out var localData))
                {
                    Debug.LogWarning($"Missing data for '{merc.MercID}'");
                    continue;
                }

                UpdateMercPassivesFromReferences(ref merc, passives);

                MercGameDataLookup[merc.MercID] = merc;
            }
        }

        /// <summary>
        /// Set the merc passives using the static data
        /// </summary>
        private void UpdateMercPassivesFromReferences(ref MercGameData merc, Dictionary<int, MercPassiveBonus> passives)
        {
            foreach (MercPassive reference in merc.Passives.ToArray())
            {
                if (passives.TryGetValue(reference.PassiveID, out MercPassiveBonus passive))
                {
                    reference.BonusValue = passive.BonusValue;
                    reference.BonusType = passive.BonusType;
                }
                else
                {
                    // Remove the passive from the merc if the data is not available
                    merc.Passives.RemoveAll(p => p.PassiveID == reference.PassiveID);

                    GMLogger.Log($"Passive {reference.PassiveID} found on merc {merc.MercID} but is not valid");
                }
            }
        }

        /// <summary>
        /// Load local merc data and convert to a lookup dictionary
        /// </summary>
        private Dictionary<MercID, MercLocalDataFileMerc> LoadLocalData()
        {
            return Resources.LoadAll<MercLocalDataFileMerc>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);
        }

        /// <summary>
        /// Fetch the aggregated dataclass for the unit
        /// </summary>
        public AggregatedMercData GetMerc(MercID key)
        {
            if (!MercData.TryGetValue(key, out var data))
                MercData[key] = data = CreateAggregatedMercInstance(key);

            return data;
        }

        private AggregatedMercData CreateAggregatedMercInstance(MercID mercId)
        {
            Func<MercGameData> getMercGameData = () => MercGameDataLookup[mercId];

            Func<MercLocalUserData> getLocalUserData = () =>
            {
                return LocalStates.GetOrCreate(x => x.ID == mercId, () => new(mercId));
            };
            Func<MercLocalDataFileMerc> getLocalGameData = () => LocalDataFiles[mercId];

            return new(mercId, getMercGameData, getLocalUserData, getLocalGameData);
        }

        /// <summary> 
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<AggregatedMercData> UnlockedMercs => LocalStates.Select(pair => GetMerc(pair.ID)).ToList();
    }
}