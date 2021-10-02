using UnityEngine;


namespace GM.Bounties
{
    public abstract class AbstractBountyShopItemPurchasePopup : Core.GMMonoBehaviour
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
            BountyShop.Data.IBountyShopItem item = App.Data.BountyShop.GetItem(ItemID);

            if (!item.InStock)
            {
                Destroy(gameObject);
            }
        }
    }
}
