using Newtonsoft.Json;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Mercs.Models
{
    public struct MercPassiveSkillData
    {
        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Type;

        public float Value;

        public int UnlockLevel;
    }
}
