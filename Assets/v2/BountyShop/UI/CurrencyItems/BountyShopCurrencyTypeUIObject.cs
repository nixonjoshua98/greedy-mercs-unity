using GM.BountyShop.Models;

namespace GM.BountyShop.UI
{
    public class BountyShopCurrencyTypeUIObject : GM.Core.GMMonoBehaviour
    {
        protected BountyShopCurrencyItemModel AssignedItem;

        public virtual void Assign(BountyShopCurrencyItemModel item)
        {
            AssignedItem = item;

            OnAssignedItem();
        }

        protected virtual void OnAssignedItem() { }
    }
}
