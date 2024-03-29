namespace GM.BountyShop.UI
{
    public abstract class AbstractBountyShopItemModal : GM.UI.PopupPanelBase
    {
        bool _IsSendingRequest;
        protected bool IsSendingRequest { get => _IsSendingRequest; set { _IsSendingRequest = value; UpdatePurchaseUI(); } }

        protected abstract void UpdatePurchaseUI();

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
