namespace GM.Armoury.Data
{
    /// <summary>
    /// User state of the armoury item
    /// </summary>
    public class ArmouryItemState
    {
        public int ID;

        public int Level;
        public int NumOwned;
        public int EvoLevel;

        public ArmouryItemState(int item)
        {
            ID = item;
        }
    }
}
