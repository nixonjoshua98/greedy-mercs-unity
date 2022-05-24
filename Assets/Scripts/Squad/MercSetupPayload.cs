namespace GM.Mercs
{
    public class MercSetupPayload
    {
        public readonly float EnergyPercentUsedToInstantiate;

        public MercSetupPayload(float energy)
        {
            EnergyPercentUsedToInstantiate = energy;
        }

        public bool IsEnergyOverload => EnergyPercentUsedToInstantiate >= 2.0f;
    }
}
