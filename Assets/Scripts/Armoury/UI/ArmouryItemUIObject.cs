namespace SRC.Armoury.UI
{
    public abstract class ArmouryItemUIObject : SRC.Core.GMMonoBehaviour
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
            return Format.BonusValue(AssignedItem.BonusType, AssignedItem.BonusValue, "orange");
        }
    }
}
