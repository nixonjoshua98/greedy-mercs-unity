using Newtonsoft.Json;

namespace GM.Mercs.Models
{
    public class MercGameDataModel
    {
        [JsonProperty(PropertyName = "mercId")]
        public MercID Id;

        [JsonProperty(PropertyName = "attackType")]
        public Data.AttackType Attack;

        public double UnlockCost;
        public double BaseDamage = -1;

        [JsonIgnore]
        public string Name;
    }
}
