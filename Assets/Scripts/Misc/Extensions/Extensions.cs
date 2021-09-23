
using System.Numerics;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace GM
{
    using AttackType = GM.Mercs.Data.AttackType;

    public static class AttackTypeExtensions
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
    public static bool TryGetKey(this JSONNode node, int key, out JSONNode result) => node.TryGetKey(key.ToString(), out result);
    public static bool TryGetKey(this JSONNode node, string key, out JSONNode result)
    {
        result = node.HasKey(key) ? node[key] : default;

        return node.HasKey(key);
    }

    public static void AddList<T>(this JSONNode node, string key, List<T> ls)
    {
        JSONArray arr = new JSONArray();

        foreach (T ele in ls)
        {
            arr.Add(ele.ToString());
        }

        node[key] = arr;       

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