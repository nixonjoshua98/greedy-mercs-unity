using GM.Armoury.Data;

namespace GM.Armoury.Requests
{
    public class UpgradeArmouryItemResponse : GM.HTTP.Requests.ServerResponse
    {
        public UserArmouryItem Item;

        public int UpgradeCost;
    }
}
