using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;
using MercID = GM.Common.Enums.MercID;

namespace GM.Core
{
    public class GenericEvents
    {
        // = Inventory = //
        public UnityEvent<BigInteger> BountyPointsChanged { get; private set; } = new UnityEvent<BigInteger>();
        public UnityEvent<BigDouble> GoldChanged { get; private set; } = new UnityEvent<BigDouble>();
        public UnityEvent<BigInteger> PrestigePointsChanged { get; private set; } = new UnityEvent<BigInteger>();
        public UnityEvent<BigInteger> ArmouryPointsChanged { get; private set; } = new UnityEvent<BigInteger>();

        // = Mercs = / /
        public UnityEvent<MercID> MercUnlocked = new UnityEvent<MercID>();
    }
}
