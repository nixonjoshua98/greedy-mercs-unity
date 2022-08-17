namespace SRC.BountyShop.UI
{
    public abstract class AbstractBountyShopItemModal : SRC.UI.PopupBase
    {
        private bool _IsSendingRequest;
        protected bool IsSendingRequest { get => _IsSendingRequest; set { _IsSendingRequest = value; UpdatePurchaseUI(); } }

        protected abstract void UpdatePurchaseUI();
    }
}
