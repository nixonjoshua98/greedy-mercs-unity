using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;

namespace GM
{
    public class GeneralEventManager
    {
        public UnityEvent<long> E_BountyPointsChange = new UnityEvent<long>();
        public UnityEvent<BigInteger> E_PrestigePointsChange = new UnityEvent<BigInteger>();
    }
}