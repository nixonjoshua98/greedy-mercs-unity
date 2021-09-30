using GM.Common.Json;
using Newtonsoft.Json;
using System.Numerics;

namespace GM.Models
{
    public class UserCurrenciesModel
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger PrestigePoints;

        public long BountyPoints { get; set; }
        public long ArmouryPoints { get; set; }
        public long BlueGems { get; set; }
    }
}
