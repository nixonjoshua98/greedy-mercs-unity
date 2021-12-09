namespace GM.Armoury.UI
{
    public abstract class ArmouryItemUIObject : Core.GMMonoBehaviour
    {
        protected int AssignedItemId = -1;

        public virtual void AssignItem(int itemId)
        {
            AssignedItemId = itemId;

            OnAssignedItem();
        }

        protected virtual void OnAssignedItem() { }

        protected Data.ArmouryItemData AssignedItem { get => App.Data.Armoury.GetItem(AssignedItemId); }
    }
}
