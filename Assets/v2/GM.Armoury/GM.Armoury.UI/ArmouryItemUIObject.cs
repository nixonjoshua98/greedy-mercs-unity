namespace GM.Armoury.UI_
{
    public abstract class ArmouryItemUIObject : Core.GMMonoBehaviour
    {
        protected int AssignedItemId = -1;

        public virtual void AssignItem(int itemId)
        {
            AssignedItemId = itemId;
        }

        protected Data.ArmouryItemData AssignedItem { get => App.Data.Armoury.GetItem(AssignedItemId); }
    }
}
