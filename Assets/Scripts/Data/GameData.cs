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
            InitialiseMercDescriptions();
        }


        public LocalMercData GetMerc(MercID merc)
        {
            return mercData[merc];
        }


        void InitialiseMercDescriptions()
        {
            LocalMercData[] fromResources = Resources.LoadAll<LocalMercData>("Mercs");

            mercData = new Dictionary<MercID, LocalMercData>();

            foreach (LocalMercData desc in fromResources)
            {
                if (mercData.ContainsKey(desc.ID))
                {
                    Debug.LogError($"Merc data contain duplicate keys!");
                    Debug.Break();
                }

                mercData[desc.ID] = desc;
            }
        }
    }


    public class GameData
    {
        static GameData Instance = null;

        public MercDataContainer Mercs;

        // = = = Static Methods = = = //
        public static void CreateInstance()
        {
            Instance = new GameData();

            Debug.Log("Created: GameData");
        }

        public static GameData Get()
        {
            return Instance;
        }

        public GameData()
        {
            Mercs = new MercDataContainer();
        }
    }
}
