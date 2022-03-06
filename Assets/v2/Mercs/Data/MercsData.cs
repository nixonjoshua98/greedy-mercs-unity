using GM.Common.Enums;
using GM.Common.Interfaces;
using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GM.LocalFiles;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass, ILocalStateFileSerializer
    {
        Dictionary<UnitID, UserMercState> UserMercs = new Dictionary<UnitID, UserMercState>();
        Dictionary<UnitID, StaticMercData> StaticMercs = new Dictionary<UnitID, StaticMercData>();

        public MercsData(IServerUserData userData, IStaticGameData staticData, LocalStateFile local)
        {
            Update(userData, staticData, local);
        }

        public void Update(IServerUserData userData, IStaticGameData staticData, LocalStateFile local)
        {
            SetDefaultMercStates(userData);

            // We may have existing local data
            if (!(local == null || local.Mercs == null))
                SetStatesFromSaveFile(local);

            SetStaticData(staticData);
        }

        public void DeleteLocalStateData()
        {
            UserMercs.Clear();
        }

        void SetStatesFromSaveFile(LocalStateFile model)
        {
            foreach (var merc in model.Mercs)
            {
                if (UserMercs.ContainsKey(merc.ID))
                {
                    UserMercs[merc.ID] = merc;
                }
                else
                {
                    GMLogger.Editor($"Merc '{merc.ID}' state found but is currently locked");
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
        Dictionary<UnitID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

        /// <summary>
        /// Fetch the data about a merc
        /// </summary>
        public StaticMercData GetGameMerc(UnitID key) => StaticMercs[key];

        /// <summary>
        /// Fetch the aggregated dataclass for the unit
        /// </summary>
        public AggregatedMercData GetMerc(UnitID key) => new AggregatedMercData(StaticMercs[key], UserMercs[key]);

        /// <summary> 
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<AggregatedMercData> UnlockedMercs => UserMercs.Select(pair => GetMerc(pair.Key)).ToList();

        /// <summary>
        /// Unit IDs for the units currently in the squad
        /// </summary>
        public List<UnitID> MercsInSquad => UserMercs.Where(x => x.Value.InSquad).Select(x => x.Key).ToList();
    }
}