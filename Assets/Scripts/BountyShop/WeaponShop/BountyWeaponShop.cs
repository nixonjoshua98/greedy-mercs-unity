using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyWeaponShop : MonoBehaviour
{
    [SerializeField] Transform scrollContent;

    [Header("Prefabs")]
    [SerializeField] GameObject WeaponShopRow;

    IEnumerator Start()
    {
        foreach (ScriptableCharacter chara in CharacterResources.Instance.Characters)
        {
            if (chara.weapons.Length > 0)
            {
                GameObject inst = Utils.UI.Instantiate(WeaponShopRow, Vector3.zero);

                inst.transform.SetParent(scrollContent);

                CharacterWeaponsRow row = inst.GetComponent<CharacterWeaponsRow>();

                row.SetCharacter(chara);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
