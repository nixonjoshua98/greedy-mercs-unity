namespace GM.Mercs
{
    public class MercSetupPayload
    {
        public readonly float RechargePercentage;

        public MercSetupPayload(float percent)
        {
            RechargePercentage = percent;
        }

        public bool IsOverCharge => RechargePercentage >= 2.0f;
    }
}
