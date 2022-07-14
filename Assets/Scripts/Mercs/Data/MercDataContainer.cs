using GM.Enums;
using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Data
{
    public class MercDataContainer : Core.GMClass
    {
        private List<UserMercLocalState> LocalStates => App.LocalStateFile.MercStates;

        private Dictionary<MercID, StaticMercData> StaticMercs = new();

        public UnityEvent<MercID> E_OnMercUnlocked { get; set; } = new UnityEvent<MercID>();

        /// <summary>
        /// Update all stored static and user data
        /// </summary>
        public void Set(List<UserMercState> userMercs, StaticMercsModel staticData)
        {
            UpdateLocalStateFile(userMercs);            
            SetStaticData(staticData);
        }

        private void UpdateLocalStateFile(List<UserMercState> mercs)
        {
            Dictionary<MercID, UserMercLocalState> states = new();

            mercs.ForEach(merc =>
            {
                var savedState = App.LocalStateFile.MercStates.FirstOrDefault(x => x.ID == merc.ID);

                states[merc.ID] = savedState ?? new UserMercLocalState(merc.ID);
            });

            App.LocalStateFile.MercStates = states.Values.ToList();
        }

        /// <summary>
        /// Add newly unlocked merc to the data we have (invokes event)
        /// </summary>
        public void AddNewUnlockedMerc(MercID mercId)
        {
            LocalStates.Add(new UserMercLocalState(mercId));

            E_OnMercUnlocked.Invoke(mercId);
        }

        public UserMercLocalState GetLocalStateOrNull(MercID id) => LocalStates.FirstOrDefault(x => x.ID == id);

        /// <summary>
        /// Update the internal static game data we have
        /// </summary>
        private void SetStaticData(StaticMercsModel model)
        {
            Dictionary<int, MercPassiveBonus> passives = model.Passives.ToDictionary(x => x.ID, x => x);

            var allLocalMercData = LoadLocalData();

            for (int i = 0; i < model.Mercs.Count; i++)
            {
                StaticMercData merc = model.Mercs[i];

                if (!allLocalMercData.TryGetValue(merc.ID, out MercScriptableObject localData))
                {
                    Debug.LogWarning($"Missing data for '{merc.ID}'");
                    continue;
                }

                merc.Icon = localData.Icon;
                merc.Prefab = localData.Prefab;

                UpdateMercPassivesFromReferences(ref merc, passives);

                StaticMercs[merc.ID] = merc;
            }
        }

        /// <summary>
        /// Set the merc passives using the static data
        /// </summary>
        private void UpdateMercPassivesFromReferences(ref StaticMercData merc, Dictionary<int, MercPassiveBonus> passives)
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

                    GMLogger.Log($"Passive {reference.PassiveID} found on merc {merc.ID} but is not valid");
                }
            }
        }

        /// <summary>
        /// Load local merc data and convert to a lookup dictionary
        /// </summary>
        private Dictionary<MercID, MercScriptableObject> LoadLocalData()
        {
            return Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);
        }

        /// <summary>
        /// Fetch the aggregated dataclass for the unit
        /// </summary>
        public AggregatedMercData GetMerc(MercID key) => new(StaticMercs[key]);

        /// <summary> 
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<AggregatedMercData> UnlockedMercs => LocalStates.Select(pair => GetMerc(pair.ID)).ToList();
    }
}