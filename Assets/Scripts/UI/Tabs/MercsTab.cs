using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;


namespace GM.UI
{
    using GreedyMercs;


    using GM.Characters;

    public class MercsTab : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] Transform scrollContent;

        [Header("Prefabs")]
        [SerializeField] GameObject characterRowObject;

        void Awake()
        {
            Events.OnCharacterUnlocked.AddListener(OnCharacterUnlocked);
        }

        void Start()
        {
            foreach (var chara in StaticData.CharacterList.mercsArray)
            {
                if (GameState.Characters.TryGetState(chara.ID, out UpgradeState _))
                    AddRow(chara);
            }
        }

        void AddRow(MercContainer chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(0);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.SetCharacter(chara);
        }

        void OnCharacterUnlocked(CharacterID chara)
        {
            AddRow(StaticData.CharacterList.Get(chara));
        }
    }
}