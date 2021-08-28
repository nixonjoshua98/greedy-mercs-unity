using UnityEngine;


namespace GM.Bounties
{
    public abstract class AbstractBountyShopItemPurchasePopup : MonoBehaviour
    {
        protected string ItemID;

        public void Setup(string id)
        {
            ItemID = id;

            SetInterfaceElements();
            UpdateInterfaceElements();
        }


        protected virtual void SetInterfaceElements()
        {

        }


        protected virtual void UpdateInterfaceElements()
        {

        }


        protected void DestroyWhenOutOfStock()
        {
            if (!UserData.Get.BountyShop.InStock(ItemID))
            {
                Destroy(gameObject);
            }
        }
    }
}
