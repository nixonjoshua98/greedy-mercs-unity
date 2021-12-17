using BountyShopArmouryItem = GM.BountyShop.Data.BountyShopArmouryItem;

namespace GM.BountyShop.UI
{
    public abstract class BSArmouryItemObject : Core.GMMonoBehaviour
    {
        protected BountyShopArmouryItem AssignedItem;

        public virtual void Assign(BountyShopArmouryItem item)
        {
            AssignedItem = item;

            OnAssignedItem();
        }


        protected virtual void OnAssignedItem() { }
    }
}
