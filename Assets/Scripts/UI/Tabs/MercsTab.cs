using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;


namespace GM.UI
{
    using GM.Events;


    using GM.Units;

    public class MercsTab : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] Transform scrollContent;

        [Header("Prefabs")]
        [SerializeField] GameObject characterRowObject;

        void Awake()
        {
            GlobalEvents.OnCharacterUnlocked.AddListener((chara) => { AddRow(chara); });
        }

        void AddRow(UnitID chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(0);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.SetCharacter(chara);
        }
    }
}