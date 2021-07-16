using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{
    public class MercDataContainer
    {
        Dictionary<MercID, LocalMercData> mercData;

        public MercDataContainer()
        {
            LocalMercData[] local = LoadLocalData();

            mercData = new Dictionary<MercID, LocalMercData>();

            foreach (LocalMercData desc in local)
            {
                mercData[desc.ID] = desc;
            }
        }


        public LocalMercData Get(MercID merc)
        {
            return mercData[merc];
        }

        LocalMercData[] LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs");
    }
}
