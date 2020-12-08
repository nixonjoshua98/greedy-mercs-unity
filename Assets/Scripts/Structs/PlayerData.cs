using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    float _Gold;

    int _Diamonds;

    public float Gold {
        get { return _Gold; }

        set
        {
            _Gold = Mathf.Max(0.0f, _Gold + value);

            EventManager.OnPlayerCurrencyChange.Invoke(_Gold, _Diamonds);
        }
    }



    public PlayerData()
    {
        Gold = 0;

        _Diamonds = 0;
    }
}
