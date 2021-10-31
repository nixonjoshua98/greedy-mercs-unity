using TMPro;
using UnityEngine;

namespace GM.Armoury.UI
{
    public class ArmouryUIController : GM.UI.Panels.Panel
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;
        public TMP_Text DamageBonusText;
        public TMP_Text CollectionText;

        void Awake()
        {
            ItemGrid.Populate(App.Data.Armoury.UserOwnedItems);
        }

        protected override void OnShown()
        {
            ItemGrid.Populate(App.Data.Armoury.UserOwnedItems);
        }

        protected override void OnHidden()
        {

        }

        void FixedUpdate()
        {
            DamageBonusText.text = $"<color=orange>{Format.Percentage(App.Cache.ArmouryMercDamageMultiplier)}</color> {Format.Bonus(BonusType.MERC_DAMAGE)}";
            CollectionText.text = $"<color=white>{App.Data.Armoury.NumUnlockedItems}/{App.Data.Armoury.NumItems}</color> Items collected";
        }
    }
}