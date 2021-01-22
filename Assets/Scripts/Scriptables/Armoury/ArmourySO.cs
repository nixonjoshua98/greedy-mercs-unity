using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GreedyMercs.Armoury.Data
{

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Armoury")]
    public class ArmourySO : ScriptableObject
    {
        [SerializeField] ArmouryWeaponSO[] WeaponArray;

        public ArmouryWeaponSO GetAtIndex(int index)
        {
            if (index > WeaponArray.Length)
                Debug.Log("Erorr: Attempting to index weapon " + index + " which does not exist in the array");

            return WeaponArray[index];                
        }

        public List<ArmouryWeaponSO> GetWeapons(WeaponType type)
        {
            return WeaponArray.Where(w => w.Type == type).ToList();
        }
    }

}