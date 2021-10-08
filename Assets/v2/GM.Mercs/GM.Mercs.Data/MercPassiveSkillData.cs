using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public struct MercPassiveSkillData
    {
        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Type;

        public float Value;

        public int UnlockLevel;
    }
}
