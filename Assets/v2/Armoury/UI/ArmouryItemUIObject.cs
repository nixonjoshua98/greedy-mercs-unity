using GM.Armoury.Data;

namespace GM.Armoury.UI
{
    public abstract class ArmouryItemUIObject : GM.UI.SlotObject
    {
        int AssignedItemId = -1;

        public virtual void AssignItem(int itemId)
        {
            AssignedItemId = itemId;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        protected ArmouryItemData AssignedItem => App.Armoury.GetItem(AssignedItemId);

        protected string GetBonusText() => Format.Bonus(AssignedItem.BonusType, AssignedItem.BonusValue, "orange");
    }
}
