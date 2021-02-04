using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SimpleJSON;

namespace GreedyMercs.Armoury.Data
{

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Armoury")]
    public class ArmourySO : ScriptableObject
    {
        [SerializeField] ArmouryItemSO[] WeaponArray;

        public void Init(JSONNode node)
        {
            for (int i = 0; i < WeaponArray.Length; ++i)
            {
                WeaponArray[i].Init(i, node[i.ToString()]);
            }
        }

        public ArmouryItemSO GetWeapon(int index)
        {
            return WeaponArray[index];
        }

        public List<ArmouryItemSO> GetWeapons(WeaponType type)
        {
            return WeaponArray.Where(w => w.Type == type).ToList();
        }

        public List<ArmouryItemSO> GetWeapons()
        {
            return WeaponArray.ToList();
        }
    }

}