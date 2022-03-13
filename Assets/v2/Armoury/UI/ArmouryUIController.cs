using UnityEngine;

namespace GM.Armoury.UI
{
    public class ArmouryUIController : GM.Core.GMMonoBehaviour
    {
        [Header("References")]
        public ArmouryItemsGridController ItemGrid;

        void Awake()
        {
            ItemGrid.Populate(App.Armoury.UserItems);
        }
    }
}