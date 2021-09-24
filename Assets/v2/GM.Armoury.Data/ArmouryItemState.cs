namespace GM.Armoury.Data
{
    public class ArmouryItemState
    {
        public int ID;

        public int Level;
        public int NumOwned;
        public int EvoLevel;

        public ArmouryItemState(int itemId)
        {
            ID = itemId;
        }
    }
}
