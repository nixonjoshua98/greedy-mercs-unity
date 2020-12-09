using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerData
{
    public float _Gold = 0.0f;

    public float Gold {
        get { return _Gold; }

        set
        {
            _Gold = Mathf.Max(0.0f, value);

            EventManager.OnPlayerCurrencyChange.Invoke(_Gold, 0);
        }
    }
}