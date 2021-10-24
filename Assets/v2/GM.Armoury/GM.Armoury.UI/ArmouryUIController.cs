using TMPro;
using UnityEngine;

namespace GM.Armoury.UI
{
    public class ArmouryUIController : GM.UI.Panels.Panel
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

        protected override void OnHidden()
        {

        }

        void FixedUpdate()
        {
            DamageBonusText.text = string.Format("{0}% Mercenary Damage", Format.Number(App.Cache.ArmouryMercDamageMultiplier * 100));
        }
    }
}