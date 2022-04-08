using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Core
{
    public class EventHandler
    {
        // = Inventory = //
        public UnityEvent<BigDouble> GoldChanged { get; private set; } = new UnityEvent<BigDouble>();
        public UnityEvent<double> PrestigePointsChanged { get; private set; } = new();
        public UnityEvent<BigInteger> ArmouryPointsChanged { get; private set; } = new UnityEvent<BigInteger>();
    }
}
