using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass
    {
        Dictionary<UnitID, UserMercState> UserMercs = new Dictionary<UnitID, UserMercState>();
        Dictionary<UnitID, StaticMercData> StaticMercs = new Dictionary<UnitID, StaticMercData>();

        public MercsData(Common.Interfaces.IServerUserData userData, StaticMercsDataResponse staticData)
        {
            SetStaticData(staticData);
            UpdateUserData(userData.UnlockedMercs);
        }

        void SetStaticData(StaticMercsDataResponse data)
        {
            Dictionary<int, MercPassive> passives = data.Passives.ToDictionary(x => x.ID, x => x);

            var allLocalMercData = LoadLocalData();

            foreach (StaticMercData merc in data.Mercs)
            {
                if (!allLocalMercData.TryGetValue(merc.ID, out MercScriptableObject localData))
                    continue;

                merc.Icon = localData.Icon;
                merc.Prefab = localData.Prefab;

                foreach (MercPassiveReference reference in merc.Passives)
                {
                    if (passives.TryGetValue(reference.PassiveID, out MercPassive passive))
                    {
                        reference.Values = passive;
                    }
                }

                merc.Passives.RemoveAll((p) => p.Values == null);

                StaticMercs[merc.ID] = merc;
            }

            AddDefaultUnits();
        }

        public void ResetLevels(int level = 1)
        {
            foreach (UserMercState merc in UserMercs.Values)
            {
                merc.Level = level;
            }
        }

        /// <summary> Load local scriptable merc data </summary>
        Dictionary<UnitID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

        void UpdateUserData(List<UserMercDataModel> ls)
        {
            foreach (var merc in ls)
            {
                if (!UserMercs.ContainsKey(merc.ID))
                    UserMercs[merc.ID] = new UserMercState();
            }

            AddDefaultUnits();
        }

        void AddDefaultUnits()
        {
            foreach (var pair in StaticMercs)
            {
                StaticMercData model = pair.Value;

                if (!UserMercs.ContainsKey(pair.Key) && model.IsDefault)
                    UserMercs[pair.Key] = new UserMercState();
            }
        }

        public void AddMercToSquad(UnitID mercId)
        {
            UserMercs[mercId].InDefaultSquad = true;
        }

        public void RemoveMercFromSquad(UnitID mercId)
        {
            UserMercs[mercId].InDefaultSquad = false;
        }

        /// <summary> Fetch the data about a merc </summary>
        public StaticMercData GetGameMerc(UnitID key) => StaticMercs[key];

        /// <summary> Fetch user merc data </summary>
        UserMercState GetUserMerc(UnitID key) => UserMercs[key];

        public MercData GetMerc(UnitID key) => new MercData(GetGameMerc(key), GetUserMerc(key));

        /// <summary> Check if the user has unlocked a merc </summary>
        public bool IsMercUnlocked(UnitID chara) => UserMercs.ContainsKey(chara);

        /// <summary> Fetch the full data for all user unlocked mercs </summary>
        public List<MercData> UnlockedMercs => UserMercs.Select(pair => GetMerc(pair.Key)).ToList();
    }
}