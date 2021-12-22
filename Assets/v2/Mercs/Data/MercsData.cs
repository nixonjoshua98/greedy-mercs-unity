using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Data
{
    public class MercsData : Core.GMClass
    {
        List<Models.MercGameDataModel> GameMercs;
        Dictionary<MercID, MercUserData> UserMercs;

        public MercsData(List<Models.MercGameDataModel> mercs)
        {
            Update(mercs);

            UserMercs = new Dictionary<MercID, MercUserData>();
        }

        /// <summary>Load local scriptable merc data</summary>
        Dictionary<MercID, MercScriptableObject> LoadLocalData() => Resources.LoadAll<MercScriptableObject>("Scriptables/Mercs").ToDictionary(ele => ele.ID, ele => ele);

        /// <summary>Update the game data</summary>
        void Update(List<Models.MercGameDataModel> data)
        {
            GameMercs = data;

           var allLocalMercData = LoadLocalData();

            foreach (var merc in GameMercs)
            {
                MercScriptableObject local = allLocalMercData[merc.Id];

                merc.Name = local.Name;
                merc.Icon = local.Icon;
                merc.Prefab = local.Prefab;
                merc.AttackType = local.AttackType;
            }
        }

        /// <summary>Fetch the data about a merc</summary>
        public Models.MercGameDataModel GetGameMerc(MercID key) => GameMercs.Where(ele => ele.Id == key).FirstOrDefault();

        /// <summary>
        /// Fetch user merc data
        /// </summary>
        MercUserData GetUserMerc(MercID key) => UserMercs.ContainsKey(key) ? UserMercs[key] : null;

        /// <summary>
        /// Update the user merc dictionary with the default values
        /// </summary>
        public void UnlockUserMerc(MercID mercId)
        {
            UserMercs[mercId] = new MercUserData { Level = 1 };

            App.Events.MercUnlocked.Invoke(mercId);
        }

        public MercData GetMerc(MercID key) => new MercData(GetGameMerc(key), GetUserMerc(key));

        /// <summary>
        /// Check if the user has unlocked a merc
        /// </summary>
        public bool IsMercUnlocked(MercID chara) => UserMercs.ContainsKey(chara);

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

        /// <summary>
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public List<MercData> UnlockedMercs => UserMercs.Where(pair => pair.Value.Level > 0).Select(pair => GetMerc(pair.Key)).ToList();
    }
}