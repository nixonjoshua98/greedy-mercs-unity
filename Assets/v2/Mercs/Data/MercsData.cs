using GM.Mercs.Models;
using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass
    {
        public List<MercGameDataModel> StaticMercsDataList => StaticMercDataLookup.Values.ToList();

        Dictionary<MercID, MercGameDataModel> StaticMercDataLookup;
        Dictionary<MercID, MercUserData> UserMercDataLookup = new Dictionary<MercID, MercUserData>();

        public MercsData(Common.Data.IServerUserData userData, Common.Data.IStaticGameData staticData)
        {
            SetStaticGameData(staticData.Mercs);
            UpdateUserData(userData.UnlockedMercs);
        }

        /// <summary> Load local scriptable merc data </summary>
        Dictionary<MercID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

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
                MercID mercId = pair.Key;
                MercGameDataModel model = pair.Value;

                if (!UserMercDataLookup.ContainsKey(mercId) && model.IsDefault)
                    UserMercDataLookup[mercId] = new MercUserData();
            }
        }

        public void AddMercToSquad(MercID mercId)
        {
            UserMercDataLookup[mercId].InDefaultSquad = true;
        }

        public void RemoveMercFromSquad(MercID mercId)
        {
            UserMercDataLookup[mercId].InDefaultSquad = false;
        }

        /// <summary> Fetch the data about a merc </summary>
        public MercGameDataModel GetGameMerc(MercID key) => StaticMercDataLookup[key];

        /// <summary> Fetch user merc data </summary>
        MercUserData GetUserMerc(MercID key) => UserMercDataLookup[key];

        public MercData GetMerc(MercID key) => new MercData(GetGameMerc(key), GetUserMerc(key));

        /// <summary> Check if the user has unlocked a merc </summary>
        public bool IsMercUnlocked(MercID chara) => UserMercDataLookup.ContainsKey(chara);

        public List<MercData> SquadMercs => UserMercDataLookup.Where(pair => pair.Value.InDefaultSquad).Select(pair => GetMerc(pair.Key)).ToList();

        /// <summary> Fetch the full data for all user unlocked mercs </summary>
        public List<MercData> UnlockedMercs => UserMercDataLookup.Select(pair => GetMerc(pair.Key)).ToList();
    }
}