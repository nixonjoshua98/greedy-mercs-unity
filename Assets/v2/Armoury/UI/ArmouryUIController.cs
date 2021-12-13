using TMPro;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Armoury.UI
{
    public class ArmouryUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;
        public TMP_Text CollectionText;

        void Awake()
        {
            ItemGrid.Populate(App.Data.Armoury.UserItems);
        }

        public override void OnShown()
        {
            ItemGrid.Populate(App.Data.Armoury.UserItems);
        }

        void FixedUpdate()
        {
            CollectionText.text = $"<color=white>{App.Data.Armoury.UserItems.Count} of {App.Data.Armoury.GameItems.Count}</color> Items collected";
        }
    }
}