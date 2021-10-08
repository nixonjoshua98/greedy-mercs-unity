using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.Data
{
    public class MercGameDataCollection
    {
        List<Models.MercGameDataModel> mercDataList;

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

        static LocalMercData[] LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs");
    }
}
