using System.Numerics;

namespace GM
{
    public static class BigNumber_Extensions
    {
        public static BigDouble ToBigDouble(this BigInteger source)
        {
            return BigDouble.Parse(source.ToString());
        }

        public static BigInteger FloorToBigInteger(this BigDouble source)
        {
            return BigInteger.Parse(source.Floor().ToString("F0"));
        }

        public static BigInteger CeilToBigInteger(this BigDouble source)
        {
            return BigInteger.Parse(source.Ceiling().ToString("F0"));
        }
    }
}
