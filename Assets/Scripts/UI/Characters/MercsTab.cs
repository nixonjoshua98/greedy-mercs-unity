﻿
using UnityEngine;


namespace GM.UI
{


    using GM.Units;

    public class MercsTab : GM.UI.Panels.Panel
    {
        [SerializeField] BuyController buyAmountController;

        [Header("Transforms")]
        [SerializeField] Transform scrollContent;

        [Header("Prefabs")]
        [SerializeField] GameObject characterRowObject;

        void Awake()
        {
            App.Data.Mercs.E_MercUnlocked.AddListener((chara) => { AddRow(chara); });
        }

        void AddRow(MercID chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(0);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.Setup(chara, buyAmountController);
        }

        protected override void OnHidden()
        {

        }

        protected override void OnShown()
        {

        }
    }
}