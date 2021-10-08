using GM.Common.Json;
using Newtonsoft.Json;
using System.Numerics;

namespace GM.Inventory.Models
{
    public class UserCurrenciesModel
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long ArmouryPoints;
        public long BlueGems;
    }
}
