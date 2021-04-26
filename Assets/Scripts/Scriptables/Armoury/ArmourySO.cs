using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using SimpleJSON;

namespace GreedyMercs.Armoury.Data
{
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Armoury")]
    public class ArmourySO : ScriptableObject
    {
        public int IronCost { get; private set; }

        [SerializeField] ArmouryItemSO[] WeaponArray;

        public void Init(JSONNode node)
        {
            IronCost = node["iron"]["cost"].AsInt;

            for (int i = 0; i < WeaponArray.Length; ++i)
            {
                WeaponArray[i].Init(i, node["gear"][i.ToString()]);
            }
        }

        public ArmouryItemSO GetWeapon(int index)
        {
            return WeaponArray[index];
        }

        public List<ArmouryItemSO> GetAllItems()
        {
            return WeaponArray.ToList();
        }
    }
}