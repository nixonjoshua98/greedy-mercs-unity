using GM.Utils;
using UnityEngine;
using System.Collections.Generic;
using ScreenSpace = GM.UI_.ScreenSpace;
using TMPro;

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