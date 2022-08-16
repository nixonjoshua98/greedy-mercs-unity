using SRC.Armoury.Data;

namespace SRC.Armoury.Requests
{
    public class UpgradeArmouryItemResponse : SRC.HTTP.Requests.ServerResponse
    {
        public UserArmouryItem Item;

        public int UpgradeCost;
    }
}
