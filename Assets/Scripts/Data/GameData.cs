using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{
    public class MercDataContainer
    {
        Dictionary<MercID, LocalMercData> descriptions;

        public MercDataContainer()
        {
            InitialiseMercDescriptions();
        }


        public LocalMercData GetDescription(MercID merc)
        {
            return descriptions[merc];
        }


        void InitialiseMercDescriptions()
        {
            LocalMercData[] fromResources = Resources.LoadAll<LocalMercData>("Mercs");

            descriptions = new Dictionary<MercID, LocalMercData>();

            foreach (LocalMercData desc in fromResources)
            {
                if (descriptions.ContainsKey(desc.ID))
                {
                    Debug.LogError($"Merc descriptions contain duplicate keys!");
                    Debug.Break();
                }

                descriptions[desc.ID] = desc;
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
