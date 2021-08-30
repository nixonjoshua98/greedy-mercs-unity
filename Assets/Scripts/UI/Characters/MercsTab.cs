
using UnityEngine;


namespace GM.UI
{
    using GM.Events;


    using GM.Units;

    public class MercsTab : PanelController
    {
        [SerializeField] BuyController buyAmountController;

        [Header("Transforms")]
        [SerializeField] Transform scrollContent;

        [Header("Prefabs")]
        [SerializeField] GameObject characterRowObject;

        void Awake()
        {
            GlobalEvents.E_OnMercUnlocked.AddListener((chara) => { AddRow(chara); });
        }

        void AddRow(MercID chara)
        {
            GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

            spawnedRow.transform.SetSiblingIndex(0);

            CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

            row.Setup(chara, buyAmountController);
        }
    }
}