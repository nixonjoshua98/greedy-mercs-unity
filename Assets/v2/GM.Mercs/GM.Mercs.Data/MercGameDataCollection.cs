using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GM.Mercs.Data
{
    public class MercGameDataCollection
    {
        List<Models.MercGameDataModel> mercDataList;

        public MercGameDataCollection(JSONNode node)
        {
            UpdateFromJSON(node);
        }

        public MercGameDataCollection(List<Models.MercGameDataModel> mercs)
        {
            Update(mercs);
        }

        public Models.MercGameDataModel Get(MercID key) => mercDataList.Where(ele => ele.Id == key).FirstOrDefault();


        public void Update(List<Models.MercGameDataModel> mercs)
        {
            mercDataList = mercs;

            LocalMercData[] allLocalMercData = LoadLocalData();

            foreach (var merc in mercDataList)
            {
                LocalMercData localMerc = allLocalMercData.Where(ele => ele.ID == merc.Id).First();

                merc.Name = localMerc.Name;
                merc.Prefab = localMerc.Prefab;
                merc.Icon = localMerc.Icon;
            }
        }

        /// <summary>
        /// Update the dictionary using a JSON (most likely from the server)
        /// </summary>
        /// <param name="json">JSON</param>
        public void UpdateFromJSON(JSONNode json)
        {
            //foreach (LocalMercData desc in LoadLocalData())
            //{
            //    JSONNode current = json[(int)desc.ID];

            //    base[desc.ID] = new MercGameData
            //    {
            //        Id = desc.ID,
            //        Name = desc.Name,
            //        Icon = desc.Icon,
            //        Prefab = desc.Prefab,

            //        Attack = (AttackType)current["attackType"].AsInt,
            //        BaseDamage = current["baseDamage"].AsDouble,
            //        UnlockCost = current["unlockCost"].AsDouble,

            //        Passives = MercGameData.ParsePassives(current["passives"].AsArray)
            //    };
            //}
        }

        static LocalMercData[] LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs");
    }
}
