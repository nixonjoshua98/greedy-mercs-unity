using GM.Armoury.Data;
using GM.HTTP;

namespace GM.Armoury.Requests
{
    public class UpgradeArmouryItemResponse : ServerResponse
    {
        public UserArmouryItem Item;

        public int UpgradeCost;
    }
}
