using System;
using System.Collections;

using UnityEngine;


namespace GreedyMercs
{
    public class MercsTab : MonoBehaviour
    {
        static MercsTab Instance = null;

        [Header("Transforms")]
        [SerializeField] Transform scrollContent;

        [Header("Controllers")]
        [SerializeField] BuyAmountController buyAmount;

        [Header("Prefabs")]
        [SerializeField] GameObject characterRowObject;

        public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

        void Awake()
        {
            Instance = this;

            Events.OnCharacterUnlocked.AddListener(OnHeroUnlocked);
        }

        IEnumerator Start()
        {
            foreach (var chara in StaticData.CharacterList.CharacterList)
            {
                if (GameState.Characters.TryGetState(chara.CharacterID, out UpgradeState _))
                    AddRow(chara);

                yield return new WaitForFixedUpdate();
            }
        }

        void AddRow(CharacterSO chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(scrollContent.childCount);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.SetCharacter(chara);
        }

        void OnHeroUnlocked(CharacterID chara)
        {
            AddRow(StaticData.CharacterList.Get(chara));
        }
    }
}