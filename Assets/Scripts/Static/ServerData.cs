using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerData
{
    static _ServerData Data = null;

    class _ServerData
    {

    }

    public static void Restore(string json)
    {
        if (Data == null)
            Data = JsonUtility.FromJson<_ServerData>(json);
    }
    public static string ToJson()
    {
        return JsonUtility.ToJson(Data);
    }

    public static bool IsValid()
    {
        return Data != null;
    }
}
