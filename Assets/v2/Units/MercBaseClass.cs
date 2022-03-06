using GM.Common.Enums;

namespace GM.Mercs
{
    public abstract class MercBaseClass : GM.Units.UnitBaseClass
    {
        public UnitID Id;

        protected MercSetupPayload SetupPayload;

        public void Init(MercSetupPayload payload)
        {
            SetupPayload = payload;
        }
    }
}
