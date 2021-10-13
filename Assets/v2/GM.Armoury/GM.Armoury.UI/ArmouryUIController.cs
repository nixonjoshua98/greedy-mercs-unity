using TMPro;
using UnityEngine;

namespace GM.Armoury.UI_
{
    public class ArmouryUIController : GM.UI.PanelController
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;
        public GameObject ArmouryItemRowObject;

        [Header("References")]
        public ArmouryItemsGridController ItemGrid;
        public TMP_Text DamageBonusText;

        void Awake()
        {
            var i = App.Data.Armoury.UserOwnedItems;
            i.AddRange(App.Data.Armoury.UserOwnedItems);
            i.AddRange(App.Data.Armoury.UserOwnedItems);
            i.AddRange(App.Data.Armoury.UserOwnedItems);
            i.AddRange(App.Data.Armoury.UserOwnedItems);

            ItemGrid.Populate(i);
        }

        protected override void OnShown()
        {
            ItemGrid.Populate(App.Data.Armoury.UserOwnedItems);
        }

        void FixedUpdate()
        {
            DamageBonusText.text = string.Format("{0}% Mercenary Damage", FormatString.Number(App.Cache.ArmouryMercDamageMultiplier * 100));
        }
    }
}