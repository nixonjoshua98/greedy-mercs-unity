using GM.Mercs.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using BonusType = GM.Common.Enums.BonusType;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Data
{
    public class MercsData
    {
        List<Models.MercGameDataModel> GameMercs;
        Dictionary<MercID, MercUserData> UserMercs;

        public UnityEvent<MercID> E_MercUnlocked = new UnityEvent<MercID>();

        public MercsData(List<Models.MercGameDataModel> mercs)
        {
            Update(mercs);

            UserMercs = new Dictionary<MercID, MercUserData>();
        }

        /// <summary>
        /// Load local scriptable merc data
        /// </summary>
        Dictionary<MercID, LocalMercData> LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs").ToDictionary(ele => ele.ID, ele => ele);

        /// <summary>
        /// Update the game data
        /// </summary>
        void Update(List<Models.MercGameDataModel> data)
        {
            GameMercs = data;

           var allLocalMercData = LoadLocalData();

            foreach (var merc in GameMercs)
            {
                LocalMercData localMerc = allLocalMercData[merc.Id];

                merc.Name = localMerc.Name;
                merc.Icon = localMerc.Icon;
                merc.Prefab = localMerc.Prefab;
            }
        }

        /// <summary>
        /// Fetch the data about a merc
        /// </summary>
        public Models.MercGameDataModel GetGameMerc(MercID key) => GameMercs.Where(ele => ele.Id == key).FirstOrDefault();

        /// <summary>
        /// Fetch user merc data
        /// </summary>
        MercUserData GetUserMerc(MercID key) => UserMercs.ContainsKey(key) ? UserMercs[key] : null;

        /// <summary>
        /// Update the user merc dictionary with the default values
        /// </summary>
        public void UnlockUserMerc(MercID key)
        {
            UserMercs[key] = new MercUserData { Level = 1 };

            E_MercUnlocked.Invoke(key);
        }

        public FullMercData GetMerc(MercID key) => new FullMercData(GetGameMerc(key), GetUserMerc(key));

        /// <summary>
        /// Check if the user has unlocked a merc
        /// </summary>
        public bool IsMercUnlocked(MercID chara) => UserMercs.ContainsKey(chara);


        public bool GetNextHero(out MercID result)
        {
            result = (MercID)(-1);

            foreach (MercID chara in System.Enum.GetValues(typeof(MercID)))
            {
                if ((int)chara >= 0 && !IsMercUnlocked(chara))
                {
                    result = chara;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Fetch the full data for all user unlocked mercs
        /// </summary>
        public FullMercData[] UnlockedMercs => UserMercs.Where(pair => pair.Value.Level > 0).Select(pair => GetMerc(pair.Key)).ToArray();


        // = = = Special Methods = = = //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (FullMercData merc in UnlockedMercs)
            {
                foreach (Models.MercPassiveDataModel passive in merc.UnlockedPassives)
                {
                    ls.Add(new KeyValuePair<BonusType, double>(passive.Type, passive.Value));
                }
            }

            return ls;
        }
    }
}