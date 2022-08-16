using UnityEngine;

namespace SRC.Armoury.UI
{
    public class ArmouryUIController : SRC.Core.GMMonoBehaviour
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;

        private void Awake()
        {
            ItemGrid.Populate(App.Armoury.UserItems);
        }
    }
}