using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Core
{
    public class GenericEvents
    {
        public UnityEvent<BigInteger> BountyPointsChanged { get; private set; } = new UnityEvent<BigInteger>();
        public UnityEvent<BigDouble> GoldChanged { get; private set; } = new UnityEvent<BigDouble>();
        public UnityEvent<BigInteger> PrestigePointsChanged { get; private set; } = new UnityEvent<BigInteger>();
        public UnityEvent<BigInteger> ArmouryPointsChanged { get; private set; } = new UnityEvent<BigInteger>();
    }
}
