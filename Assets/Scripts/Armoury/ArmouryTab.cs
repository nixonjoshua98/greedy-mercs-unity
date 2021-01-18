using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Armoury.UI
{
    using Scriptables.Weapons;

    public class ArmouryTab : MonoBehaviour
    {
        [SerializeField] WeaponListSO WeaponList;

        [Header("Scene References")]
        [SerializeField] GameObject WeaponFrameParent;

        [Header("Components")]
        [SerializeField] GameObject WeaponFrameObject;

        void Start()
        {
            for (int i = 0; i < WeaponList.Count; ++i)
            {
                WeaponSO weapon = WeaponList.Get(i);

                GameObject inst = Utils.UI.Instantiate(WeaponFrameObject, WeaponFrameParent.transform, Vector3.zero);

                WeaponFrame frame = inst.GetComponent<WeaponFrame>();

                frame.Init(weapon.icon);
            }
        }
    }
}