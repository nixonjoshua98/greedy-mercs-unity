using GM.Common.Enums;

namespace GM.Mercs
{
    public abstract class MercBaseClass : GM.Units.UnitBase
    {
        public MercID Id;

        protected MercSetupPayload SetupPayload;

        public void Init(MercSetupPayload payload)
        {
            SetupPayload = payload;
        }
    }
}
