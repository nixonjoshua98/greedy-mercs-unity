using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;

    public class ArmouryPanel : MonoBehaviour
    {
        [SerializeField] Text weaponPointText;

        [Header("Weapon Parents")]
        [SerializeField] Transform swordParent;

        IEnumerator Start()
        {
            List<ArmouryWeaponSO> swords = StaticData.Armoury.GetWeapons(WeaponType.SWORD);

            yield return Populate(swordParent, swords);
        }

        IEnumerator Populate(Transform parent, List<ArmouryWeaponSO> weapons)
        {
            yield return new WaitForEndOfFrame();
        }

        void FixedUpdate()
        {
            weaponPointText.text = GameState.Player.weaponPoints.ToString();
        }
    }
}