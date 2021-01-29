using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;


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

            Events.OnCharacterUnlocked.AddListener(OnCharacterUnlocked);
        }

        void Start()
        {
            foreach (var chara in StaticData.CharacterList.CharacterList)
            {
                if (GameState.Characters.TryGetState(chara.CharacterID, out UpgradeState _))
                    AddRow(chara);
            }
        }

        // === Static Methods ===
        public static void AddBuyAmountListener(UnityAction<int> callback)
        {
            Instance.buyAmount.OnBuyAmountChanged.AddListener(callback);
        }

        void AddRow(CharacterSO chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(1);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.SetCharacter(chara);
        }

        void OnCharacterUnlocked(CharacterID chara)
        {
            AddRow(StaticData.CharacterList.Get(chara));
        }
    }
}