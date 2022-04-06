using GM.Armoury.Data;

namespace GM.Armoury.UI
{
    public abstract class ArmouryItemUIObject : GM.UI.SlotObject
    {
        private int AssignedItemId = -1;

        public virtual void AssignItem(int itemId)
        {
            AssignedItemId = itemId;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        protected AggregatedArmouryItem AssignedItem => App.Armoury.GetItem(AssignedItemId);

        protected string GetBonusText()
        {
            return Format.Bonus(AssignedItem.BonusType, AssignedItem.BonusValue, "orange");
        }
    }
}
