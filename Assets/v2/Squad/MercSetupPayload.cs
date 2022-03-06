namespace GM.Mercs
{
    public class MercSetupPayload
    {
        public readonly float EnergyPercentUsedToInstantiate;

        public MercSetupPayload(float energy)
        {
            EnergyPercentUsedToInstantiate = energy;
        }
    }
}
