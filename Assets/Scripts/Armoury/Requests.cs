using GM.Armoury.Data;
using GM.HTTP;

namespace GM.Armoury.Requests
{
    public class UpgradeArmouryItemResponse : GM.HTTP.Requests.ServerResponse
    {
        public UserArmouryItem Item;

        public int UpgradeCost;
    }
}
