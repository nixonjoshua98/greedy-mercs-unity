using GM.Common.Json;
using Newtonsoft.Json;
using System.Numerics;

namespace GM.Inventory.Models
{
    public class UserCurrenciesModel
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger PrestigePoints { get; set; }
        public long BountyPoints { get; set; }
        public long ArmouryPoints { get; set; }
        public long BlueGems { get; set; }
    }
}
