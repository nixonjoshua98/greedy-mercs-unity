using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using MercID = GM.Common.Enums.MercID;
using GM.Mercs.Models;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass
    {
        public List<MercGameDataModel> StaticMercsDataList => StaticMercDataLookup.Values.ToList();


        Dictionary<MercID, MercGameDataModel> StaticMercDataLookup;
        Dictionary<MercID, MercUserData> UserMercDataLookup = new Dictionary<MercID, MercUserData>();

        public MercsData(List<MercGameDataModel> mercs)
        {
            Update(mercs);
        }

        /// <summary>Load local scriptable merc data</summary>
        Dictionary<MercID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

        /// <summary>Update the game data</summary>
        void Update(List<MercGameDataModel> data)
        {
            StaticMercDataLookup = data.ToDictionary(x => x.Id, x => x);

            var allLocalMercData = LoadLocalData();

            foreach (var pair in StaticMercDataLookup)
            {
                MercID mercId = pair.Key;
                MercGameDataModel model = pair.Value;

                MercScriptableObject local = allLocalMercData[model.Id];

                model.Name = local.Name;
                model.Icon = local.Icon;
                model.Prefab = local.Prefab;
                model.AttackType = local.AttackType;

                // Add default mercs
                if (!UserMercDataLookup.ContainsKey(mercId) && model.IsDefault)
                {
                    UserMercDataLookup[mercId] = new MercUserData() { Level = 1, InSquad = false };
                }
            }
        }

        public void AddMercToSquad(MercID mercId)
        {
            UserMercDataLookup[mercId].InSquad = true;

            App.Events.OnMercAddedToSquad.Invoke(mercId);
        }

        public void RemoveMercFromSquad(MercID mercId)
        {
            UserMercDataLookup[mercId].InSquad = false;

            App.Events.OnMercRemovedFromSquad.Invoke(mercId);
        }

        /// <summary>Fetch the data about a merc</summary>
        public Models.MercGameDataModel GetGameMerc(MercID key) => StaticMercDataLookup[key];

        /// <summary> Fetch user merc data </summary>
        MercUserData GetUserMerc(MercID key)
        {
            if (!UserMercDataLookup.ContainsKey(key))
            {
                GMLogger.Editor($"Merc '{key}' is not present in the user data");
            }

            return UserMercDataLookup[key];
        }

        /// <summary>
        /// Update the user merc dictionary with the default values
        /// </summary>
        public void UnlockUserMerc(MercID mercId)
        {
            UserMercDataLookup[mercId] = new MercUserData { Level = 1 };
        }

        public MercData GetMerc(MercID key) => new MercData(GetGameMerc(key), GetUserMerc(key));

        /// <summary>
        /// Check if the user has unlocked a merc
        /// </summary>
        public bool IsMercUnlocked(MercID chara) => UserMercDataLookup.ContainsKey(chara);

        public bool GetNextHero(out MercID result)
        {
            result = default;

            var ls = new List<MercID>() {
                MercID.SKELETON_ARCHER,
                MercID.STONE_GOLEM, 
                MercID.WRAITH
            };

            foreach (MercID merc in ls)
            {
                if (!IsMercUnlocked(merc))
                {
                    result = merc;
                    return true;
                }
            }

            return false;
        }

        public List<MercData> SquadMercs => UserMercDataLookup.Where(pair => pair.Value.InSquad).Select(pair => GetMerc(pair.Key)).ToList();

        /// <summary>
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<MercData> UnlockedMercs => UserMercDataLookup.Where(pair => pair.Value.Level > 0).Select(pair => GetMerc(pair.Key)).ToList();
    }
}