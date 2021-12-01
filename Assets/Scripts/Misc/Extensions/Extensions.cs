
using GM.Common.Enums;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace GM
{
    public static class AttackTypeExtensions
    {
        public static BonusType Bonus(this AttackType val)
        {
            switch (val)
            {
                case AttackType.MELEE:
                    return BonusType.MELEE_DAMAGE;

                case AttackType.RANGED:
                    return BonusType.RANGED_DAMAGE;

                default:
                    return BonusType.MELEE_DAMAGE;
            }
        }
    }
}



public static class CameraExtensions
{
    public static Vector2 MinBounds(this Camera camera)
    {
        Vector2 v2 = camera.Extents();

        return camera.transform.position - new Vector3(v2.x, v2.y);
    }

    public static Vector2 MaxBounds(this Camera camera)
    {
        Vector2 v2 = camera.Extents();

        return camera.transform.position + new Vector3(v2.x, v2.y);
    }

    public static Vector2 Extents(this Camera camera)
    {
        return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
    }
}


public static class BigNumberExtensions
{
    public static BigDouble ToBigDouble(this BigInteger val)
    {
        return BigDouble.Parse(val.ToString());
    }

    public static BigInteger FloorToBigInteger(this BigDouble @this)
    {
        return BigInteger.Parse(@this.Floor().ToString("F0"));
    }

    public static BigInteger ToBigInteger(this BigDouble val)
    {
        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }
}