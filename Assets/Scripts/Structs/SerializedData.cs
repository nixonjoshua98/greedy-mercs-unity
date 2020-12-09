using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializedData
{
    public float Gold;

    public static SerializedData Create(PlayerData playerData)
    {
        return new SerializedData()
        {
            Gold = playerData.Gold
        };
    }

    public PlayerData GetPlayerData()
    {
        return new PlayerData()
        {
            Gold = Gold
        };
    }
}
