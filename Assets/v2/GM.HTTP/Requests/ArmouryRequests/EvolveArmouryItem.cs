namespace GM.HTTP.Requests
{
    public class MergeArmouryItemRequest : IServerRequest
    {
        public readonly int ItemId;

        public MergeArmouryItemRequest(int item)
        {
            ItemId = item;
        }
    }


    public class MergeArmouryItemResponse : ServerResponse
    {
        public Armoury.Models.ArmouryItemUserDataModel Item;
    }
}
