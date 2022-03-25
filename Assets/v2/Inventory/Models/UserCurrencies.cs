using GM.Common.Json;
using Newtonsoft.Json;
using System.Numerics;

namespace GM.Inventory.Models
{
    public class UserCurrencies
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long ArmouryPoints;
    }
}