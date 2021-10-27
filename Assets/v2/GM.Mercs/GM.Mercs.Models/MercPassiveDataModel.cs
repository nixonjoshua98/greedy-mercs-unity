using Newtonsoft.Json;

namespace GM.Mercs.Models
{
    public struct MercPassiveDataModel
    {
        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Type;

        [JsonProperty(PropertyName = "bonusValue")]
        public float Value;

        public int UnlockLevel;
    }
}
