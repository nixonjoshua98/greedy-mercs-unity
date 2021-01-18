using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


namespace Data.Weapons
{
    [System.Serializable]
    public class WeaponStaticData
    {
        public int buyCost = -1;

        public int mergeCost;

        public int maxOwned;

        public float damageBonus;

        public Dictionary<int, int> mergeRecipe;
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

                Dictionary<int, int> recipe = new Dictionary<int, int>();

                foreach (string recipeWeaponIndex in node[weaponIndex]["mergeRecipe"].Keys)
                {
                    recipe[int.Parse(recipeWeaponIndex)] = node[weaponIndex]["mergeRecipe"][recipeWeaponIndex].AsInt;
                }

                data.mergeRecipe = recipe;

                weapons.Add(data);
            }
        }

        public WeaponStaticData GetWeaponAtIndex(int weaponIndex)
        {
            return weapons[weaponIndex];
        }
    }
}
