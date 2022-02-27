using GM.Mercs.Models;
using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass
    {
        Dictionary<UnitID, MercGameDataModel> StaticMercDataLookup;
        Dictionary<UnitID, MercUserData> UserMercDataLookup = new Dictionary<UnitID, MercUserData>();

        public MercsData(Common.Interfaces.IServerUserData userData, List<MercGameDataModel> staticData)
        {
            SetStaticGameData(staticData);
            UpdateUserData(userData.UnlockedMercs);
        }

        public void ResetLevels(int level = 1)
        {
            foreach (MercUserData merc in UserMercDataLookup.Values)
            {
                merc.Level = level;
            }
        }

        /// <summary> Load local scriptable merc data </summary>
        Dictionary<UnitID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

        /// <summary> Update the game data </summary>
        void SetStaticGameData(List<MercGameDataModel> data)
        {
            StaticMercDataLookup = data.ToDictionary(x => x.Id, x => x);

            UpdateStaticGameDataWithLocalData();

            AddDefaultUnits();
        }

        void UpdateStaticGameDataWithLocalData()
        {
            var allLocalMercData = LoadLocalData();

            foreach (var pair in StaticMercDataLookup)
            {
                MercGameDataModel model = pair.Value;

                MercScriptableObject local = allLocalMercData[pair.Key];

                model.Name = local.Name;
                model.Icon = local.Icon;
                model.Prefab = local.Prefab;
                model.AttackType = local.AttackType;
            }
        }

        void UpdateUserData(List<UserMercDataModel> ls)
        {
            foreach (var merc in ls)
            {
                if (!UserMercDataLookup.ContainsKey(merc.Id))
                    UserMercDataLookup[merc.Id] = new MercUserData();
            }

            AddDefaultUnits();
        }

        void AddDefaultUnits()
        {
            foreach (var pair in StaticMercDataLookup)
            {
                UnitID mercId = pair.Key;
                MercGameDataModel model = pair.Value;

                if (!UserMercDataLookup.ContainsKey(mercId) && model.IsDefault)
                    UserMercDataLookup[mercId] = new MercUserData();
            }
        }

        public void AddMercToSquad(UnitID mercId)
        {
            UserMercDataLookup[mercId].InDefaultSquad = true;
        }

        public void RemoveMercFromSquad(UnitID mercId)
        {
            UserMercDataLookup[mercId].InDefaultSquad = false;
        }

        /// <summary> Fetch the data about a merc </summary>
        public MercGameDataModel GetGameMerc(UnitID key) => StaticMercDataLookup[key];

        /// <summary> Fetch user merc data </summary>
        MercUserData GetUserMerc(UnitID key) => UserMercDataLookup[key];

        public MercData GetMerc(UnitID key) => new MercData(GetGameMerc(key), GetUserMerc(key));

        /// <summary> Check if the user has unlocked a merc </summary>
        public bool IsMercUnlocked(UnitID chara) => UserMercDataLookup.ContainsKey(chara);

        /// <summary> Fetch the full data for all user unlocked mercs </summary>
        public List<MercData> UnlockedMercs => UserMercDataLookup.Select(pair => GetMerc(pair.Key)).ToList();
    }
}