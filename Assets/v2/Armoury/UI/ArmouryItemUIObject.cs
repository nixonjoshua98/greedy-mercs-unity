namespace GM.Armoury.UI
{
    public abstract class ArmouryItemUIObject : Core.GMMonoBehaviour
    {
        protected int AssignedItemId = -1;

        public virtual void AssignItem(int itemId)
        {
            AssignedItemId = itemId;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        protected Data.ArmouryItemData AssignedItem => App.Data.Armoury.GetItem(AssignedItemId);
    }
}
