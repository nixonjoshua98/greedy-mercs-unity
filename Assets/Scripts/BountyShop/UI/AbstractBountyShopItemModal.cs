namespace GM.BountyShop.UI
{
    public abstract class AbstractBountyShopItemModal : GM.UI.PopupBase
    {
        bool _IsSendingRequest;
        protected bool IsSendingRequest { get => _IsSendingRequest; set { _IsSendingRequest = value; UpdatePurchaseUI(); } }

        protected abstract void UpdatePurchaseUI();
    }
}
