using UnityEngine;

namespace GM.Armoury.UI
{
    public class ArmouryUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;

        void Awake()
        {
            ItemGrid.Populate(App.DataContainers.Armoury.UserItems);
        }

        public override void OnShown()
        {
            ItemGrid.Populate(App.DataContainers.Armoury.UserItems);
        }
    }
}