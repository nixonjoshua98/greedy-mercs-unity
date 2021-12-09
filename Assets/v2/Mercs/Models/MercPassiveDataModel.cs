using Newtonsoft.Json;
using BonusType = GM.Common.Enums.BonusType;

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
