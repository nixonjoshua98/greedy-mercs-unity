using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


namespace WeaponData
{
    [System.Serializable]
    public struct WeaponStaticData
    {
        public int cost;

        public int maxOwned;

        public float damageBonus;

        public int tier;
    }


    public class Weapons
    {
        List<WeaponStaticData> weapons;

        public Weapons(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            weapons = new List<WeaponStaticData>();

            foreach (string weaponIndex in node.Keys)
            {
                var data = JsonUtility.FromJson<WeaponStaticData>(node[weaponIndex].ToString());

                data.tier = int.Parse(weaponIndex) + 1;

                weapons.Add(data);
            }
        }

        public WeaponStaticData Get(int weaponIndex)
        {
            return weapons[weaponIndex];
        }
    }
}
