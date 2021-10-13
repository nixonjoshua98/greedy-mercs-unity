using TMPro;
using UnityEngine;

namespace GM.Armoury.UI_
{
    public class ArmouryUIController : GM.UI.PanelController
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;
        public TMP_Text DamageBonusText;

        void Awake()
        {
            ItemGrid.Populate(App.Data.Armoury.UserOwnedItems);
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