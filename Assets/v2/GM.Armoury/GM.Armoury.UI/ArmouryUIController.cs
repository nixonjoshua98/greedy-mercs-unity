using TMPro;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Armoury.UI
{
    public class ArmouryUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;
        public TMP_Text DamageBonusText;
        public TMP_Text CollectionText;

        void Awake()
        {
            ItemGrid.Populate(App.Data.Armoury.UserOwnedItems);
        }

        public override void OnShown()
        {
            ItemGrid.Populate(App.Data.Armoury.UserOwnedItems);
        }

        void FixedUpdate()
        {
            DamageBonusText.text = $"<color=orange>{Format.Percentage(App.Cache.ArmouryMercDamageMultiplier)}</color> {Format.Bonus(BonusType.MERC_DAMAGE)}";
            CollectionText.text = $"<color=white>{App.Data.Armoury.NumUnlockedItems} of {App.Data.Armoury.NumItems}</color> Items collected";
        }
    }
}