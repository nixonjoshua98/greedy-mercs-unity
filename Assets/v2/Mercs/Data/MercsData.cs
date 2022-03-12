using GM.Common.Enums;
using GM.LocalFiles;
using GM.Mercs.ScriptableObjects;
using GM.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass, ILocalStateFileSerializer
    {
        Dictionary<MercID, UserMercState> UserMercs = new Dictionary<MercID, UserMercState>();
        Dictionary<MercID, StaticMercData> StaticMercs = new Dictionary<MercID, StaticMercData>();

        public int MaxSquadSize { get; private set; } = 5;

        public void Set(IServerUserData userData, IStaticGameData staticData, LocalStateFile local)
        {
            SetDefaultMercStates(userData);

            // We may have existing local data
            if (!(local == null || local.Mercs == null))
                SetStatesFromSaveFile(local);

            SetStaticData(staticData);
            UpdatePersistantLocalFile(App.PersistantLocalFile);
        }

        public void DeleteLocalStateData()
        {
            UserMercs.Clear();
        }

        void UpdatePersistantLocalFile(LocalPersistantFile file)
        {
            file.SquadMercIDs.RemoveWhere(id => !UserMercs.ContainsKey(id));

            if (IsSquadFull) 
                file.SquadMercIDs.Clear();
        }

        void SetStatesFromSaveFile(LocalStateFile model)
        {
            foreach (var merc in model.Mercs)
            {
                if (UserMercs.ContainsKey(merc.ID))
                {
                    UserMercs[merc.ID] = merc;
                }
            }
        }

        void SetDefaultMercStates(IServerUserData data)
        {
            foreach (var merc in data.UnlockedMercs)
            {
                UserMercs[merc.ID] = new UserMercState(merc.ID);
            }
        }

        public void AddNewUnlockedMerc(MercID mercId)
        {
            UserMercs.Add(mercId, new(mercId));

            App.E_OnMercUnlocked.Invoke(mercId);
        }

        public void UpdateLocalSaveFile(ref LocalStateFile model)
        {
            model.Mercs = UserMercs.Values.ToList();
        }

        void SetStaticData(IStaticGameData data)
        {
            Dictionary<int, MercPassive> passives = data.Mercs.Passives.ToDictionary(x => x.ID, x => x);
            List<StaticMercData> mercs = data.Mercs.Mercs;

            var allLocalMercData = LoadLocalData();

            for (int i = 0; i < mercs.Count; i++)
            {
                StaticMercData merc = mercs[i];

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
        void UpdateMercPassivesFromReferences(ref StaticMercData merc, in Dictionary<int, MercPassive> passives)
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
        Dictionary<MercID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

        /// <summary>
        /// Fetch the data about a merc
        /// </summary>
        public StaticMercData GetGameMerc(MercID key) => StaticMercs[key];

        public HashSet<MercID> SquadMercs => App.PersistantLocalFile.SquadMercIDs;

        public bool IsSquadFull => SquadMercs.Count >= MaxSquadSize;


        /// <summary>
        /// Fetch the aggregated dataclass for the unit
        /// </summary>
        public AggregatedMercData GetMerc(MercID key) => new AggregatedMercData(StaticMercs[key], UserMercs[key]);

        /// <summary> 
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<AggregatedMercData> UnlockedMercs => UserMercs.Select(pair => GetMerc(pair.Key)).ToList();

        /// <summary>
        /// Unit IDs for the units currently in the squad
        /// </summary>
        public List<MercID> MercsInSquad => UserMercs.Where(x => x.Value.InSquad).Select(x => x.Key).ToList();
    }
}