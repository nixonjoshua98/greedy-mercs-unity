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
            Events.OnCharacterUnlocked.AddListener((chara) => { AddRow(chara); });
        }

        void AddRow(CharacterID chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(0);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.SetCharacter(chara);
        }
    }
}