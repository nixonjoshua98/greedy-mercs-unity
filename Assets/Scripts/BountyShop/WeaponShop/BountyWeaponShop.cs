using System.Collections;

using UnityEngine;

namespace BountyUI
{
    public class BountyWeaponShop : MonoBehaviour
    {
        [SerializeField] Transform scrollContent;

        [Header("Prefabs")]
        [SerializeField] GameObject WeaponSelection;

        IEnumerator Start()
        {
            foreach (ScriptableCharacter chara in CharacterResources.Instance.Characters)
            {
                if (chara.weapons.Length > 0)
                {
                    GameObject inst = Utils.UI.Instantiate(WeaponSelection, Vector3.zero);

                    inst.transform.SetParent(scrollContent);

                    WeaponSelection row = inst.GetComponent<WeaponSelection>();

                    row.SetCharacter(chara);
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}