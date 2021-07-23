using System;
using System.Numerics;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace GM
{
    using AttackType = Units.AttackType;

    public static class Enum_Extensions
    {
        public static BonusType ToBonusType(this AttackType val)
        {
            switch (val)
            {
                case AttackType.MELEE:
                    return BonusType.MELEE_DAMAGE;

                case AttackType.RANGED:
                    return BonusType.RANGED_DAMAGE;

                default:
                    Debug.LogError($"Invalid AttackType - Defaulting to Melee.");

                    return BonusType.MELEE_DAMAGE;
            }
        }
    }
}



public static class DictionaryExtensions
{
    public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue fallback)
    {
        return dict.TryGetValue(key, out var value) ? value : fallback;
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


public static class JSONNodeExtensions
{
    public static bool TryGet(this JSONNode node, string key, out JSONNode result)
    {
        result = node.HasKey(key) ? node[key] : default;

        return node.HasKey(key);
    }

    public static void Update(this JSONNode original, JSONNode updateNode)
    {
        foreach (string key in updateNode.Keys)
        {
            original[key] = updateNode[key];
        }
    }
}


public static class BigNumberExtensions
{
    public static BigDouble ToBigDouble(this BigInteger val)
    {
        return BigDouble.Parse(val.ToString());
    }

    public static BigInteger ToBigInteger(this BigDouble val)
    {
        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }
}


public static class Extensions
{
    public static DateTime ToUnixDatetime(this long val)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(val).DateTime;
    }
}