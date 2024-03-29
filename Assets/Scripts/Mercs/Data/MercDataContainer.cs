using GM.Common.Enums;
using GM.LocalFiles;
using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Data
{
    public class MercDataContainer : Core.GMClass
    {
        private readonly Dictionary<MercID, UserMercState> UserMercs = new Dictionary<MercID, UserMercState>();
        private readonly Dictionary<MercID, StaticMercData> StaticMercs = new Dictionary<MercID, StaticMercData>();

        public UnityEvent<MercID> E_OnMercUnlocked { get; set; } = new UnityEvent<MercID>();

        public readonly int MaxSquadSize = 5;

        /// <summary>
        /// Update all stored static and user data
        /// </summary>
        public void Set(List<UserMercDataModel> userMercs, StaticMercsModel staticData, LocalStateFile local)
        {
            SetDefaultMercStates(userMercs);

            // We may have existing local data
            if (!(local == null || local.Mercs == null))
                SetStatesFromSaveFile(local);

            SetStaticData(staticData);
            UpdatePersistantLocalFile(App.PersistantLocalFile);
        }

        /// <summary>
        /// Delete all local state data (data which exists only in the current prestige)
        /// </summary>
        public void DeleteLocalStateData()
        {
            UserMercs.Clear();
        }

        /// <summary>
        /// Perform some checks on the persistant file to avoid invalid data
        /// </summary>
        private void UpdatePersistantLocalFile(LocalPersistantFile file)
        {
            file.SquadMercIDs.RemoveWhere(id => !UserMercs.ContainsKey(id));

            if (IsSquadFull)
                file.SquadMercIDs.Clear();
        }

        /// <summary>
        /// Update the merc states (level etc.) from the local save file
        /// </summary>
        private void SetStatesFromSaveFile(LocalStateFile model)
        {
            foreach (var merc in model.Mercs)
            {
                if (UserMercs.ContainsKey(merc.ID))
                {
                    UserMercs[merc.ID] = merc;
                }
            }
        }

        /// <summary>
        /// Sets default state for all unlocked mercs
        /// </summary>
        private void SetDefaultMercStates(List<UserMercDataModel> mercs)
        {
            foreach (var merc in mercs)
            {
                UserMercs[merc.ID] = new UserMercState(merc.ID);
            }
        }

        /// <summary>
        /// Add newly unlocked merc to the data we have (invokes event)
        /// </summary>
        public void AddNewUnlockedMerc(MercID mercId)
        {
            UserMercs.Add(mercId, new(mercId));

            E_OnMercUnlocked.Invoke(mercId);
        }

        public bool TryGetMercState(MercID id, out UserMercState state) => UserMercs.TryGetValue(id, out state);

        /// <summary>
        /// Temp
        /// </summary>
        public void UpdateLocalSaveFile(ref LocalStateFile model)
        {
            model.Mercs = UserMercs.Values.ToList();
        }

        /// <summary>
        /// Update the internal static game data we have
        /// </summary>
        private void SetStaticData(StaticMercsModel model)
        {
            Dictionary<int, MercPassive> passives = model.Passives.ToDictionary(x => x.ID, x => x);

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

                UpdateMercPassivesFromReferences(ref merc, in passives);

                StaticMercs[merc.ID] = merc;
            }
        }

        /// <summary>
        /// Set the merc passives using the static data
        /// </summary>
        private void UpdateMercPassivesFromReferences(ref StaticMercData merc, in Dictionary<int, MercPassive> passives)
        {
            foreach (MercPassiveReference reference in merc.Passives)
            {
                if (passives.TryGetValue(reference.PassiveID, out MercPassive passive))
                {
                    reference.Values = passive;
                }
            }

            merc.Passives.RemoveAll((p) => p.Values == null);
        }

        /// <summary>
        /// Load local merc data and convert to a lookup dictionary
        /// </summary>
        private Dictionary<MercID, MercScriptableObject> LoadLocalData()
        {
            return Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);
        }

        /// <summary>
        /// Fetch the squad mercs (stored in the PersistantLocalFile)
        /// </summary>
        public HashSet<MercID> SquadMercs => App.PersistantLocalFile.SquadMercIDs;

        /// <summary>
        /// Check if the squad is currently full (no more mercs should be added)
        /// </summary>
        public bool IsSquadFull => SquadMercs.Count >= MaxSquadSize;

        /// <summary>
        /// Fetch the aggregated dataclass for the unit
        /// </summary>
        public AggregatedMercData GetMerc(MercID key) => new(StaticMercs[key]);

        /// <summary> 
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<AggregatedMercData> UnlockedMercs => UserMercs.Select(pair => GetMerc(pair.Key)).ToList();

        /// <summary>
        /// Units which the user has decided to have in their squad
        /// </summary>
        public List<MercID> MercsInSquad => UserMercs.Where(x => GetMerc(x.Key).InSquad).Select(x => x.Key).ToList();
    }
}