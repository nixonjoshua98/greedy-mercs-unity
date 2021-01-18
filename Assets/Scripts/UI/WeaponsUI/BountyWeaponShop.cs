using System.Collections;

using UnityEngine;

namespace GreedyMercs
{
    public class BountyWeaponShop : MonoBehaviour
    {
        [SerializeField] Transform scrollContent;

        [Header("References")]
        [SerializeField] WeaponSelection selection;

        [Header("Prefabs")]
        [SerializeField] GameObject CharacterButton;

        void Start()
        {
            foreach (CharacterSO chara in StaticData.CharacterList.CharacterList)
            {
                if (chara.weapons.Length > 0)
                {
                    var temp = chara;

                    GameObject inst = Utils.UI.Instantiate(CharacterButton, Vector3.zero);

                    var component = inst.GetComponent<CharacterIcon>();

                    component.SetCharacter(chara);

                    component.button.onClick.AddListener(delegate { OnClick(temp); });

                    inst.transform.SetParent(scrollContent);
                }
            }
        }

        void OnClick(CharacterSO character)
        {
            selection.SetCharacter(character);
        }
    }
}