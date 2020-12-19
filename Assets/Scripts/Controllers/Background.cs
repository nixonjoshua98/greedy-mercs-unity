﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SpriteColourPair
{
    public Sprite Background;
    public Color Color;
}


public class Background : MonoBehaviour
{
    /* ===
     * This class handles the background transitions for the game, transitions occur every 25 stages
     * 
     * Note
     *      Feedback told me that it looks better without the transition
     * ===
     */

    [SerializeField] SpriteColourPair[] backgrounds;

    void Awake()
    {
        EventManager.OnNewStageStarted.AddListener(OnNewStageStarted);
    }

    void Start()
    {
        ChangeBackground();
    }

    void OnNewStageStarted()
    {
        if (GameState.stage.stage % 25 == 0)
        {
            ChangeBackground();
        }
    }

    void ChangeBackground()
    {
        SpriteColourPair pair = backgrounds[Mathf.FloorToInt(GameState.stage.stage / 25) % backgrounds.Length];

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Camera.main.backgroundColor = pair.Color;

        sr.sprite = pair.Background;
    }
}