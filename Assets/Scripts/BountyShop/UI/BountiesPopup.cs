using SRC.UI.Navigation;
using UnityEngine;

namespace SRC.BountyShop.UI
{
    public class BountiesPopup : MonoBehaviour
    {
        [SerializeField] BountyShopTab Shop;

        [SerializeField] TabGroup TabGroup;

        public void Initialize(string selectedTab)
        {
            TabGroup.Select(selectedTab);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
