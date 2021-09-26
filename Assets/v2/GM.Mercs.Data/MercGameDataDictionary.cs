using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs.Data
{
    public class MercGameDataDictionary : Dictionary<MercID, MercGameData>
    {
        public MercGameDataDictionary(JSONNode node)
        {
            UpdateFromJSON(node);
        }

        /// <summary>
        /// Update the dictionary using a JSON (most likely from the server)
        /// </summary>
        /// <param name="json">JSON</param>
        public void UpdateFromJSON(JSONNode json)
        {
            foreach (LocalMercData desc in LoadLocalData())
            {
                JSONNode current = json[(int)desc.ID];

                base[desc.ID] = new MercGameData
                {
                    ID = desc.ID,
                    Name = desc.Name,
                    Icon = desc.Icon,
                    Prefab = desc.Prefab,

                    Attack = (AttackType)current["attackType"].AsInt,
                    BaseDamage = current["baseDamage"].AsDouble,
                    UnlockCost = current["unlockCost"].AsDouble,

                    Passives = MercGameData.ParsePassives(current["passives"].AsArray)
                };
            }
        }

        static LocalMercData[] LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs");
    }
}
