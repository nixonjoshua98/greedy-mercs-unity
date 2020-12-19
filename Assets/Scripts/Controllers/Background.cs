using System.Collections;
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
    [SerializeField] Transition transition;

    [SerializeField] SpriteColourPair[] backgrounds;

    bool isAvailable;

    void Awake()
    {
        isAvailable = true;

        EventManager.OnNewStageStarted.AddListener(OnNewStageStarted);
    }

    void Start()
    {
        ChangeBackground();
    }

    void OnNewStageStarted()
    {
        if (isAvailable)
        {
            if (GameState.stage.stage % 25 == 0)
            {
                isAvailable = false;

                transition.Run(WhileHidden, OnTransitionFinished, 0.5f);
            }
        }
    }

    void WhileHidden()
    {
        ChangeBackground();
    }

    void OnTransitionFinished()
    {
        isAvailable = true;
    }

    void ChangeBackground()
    {
        SpriteColourPair pair = backgrounds[Mathf.FloorToInt(GameState.stage.stage / 25) % backgrounds.Length];

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Camera.main.backgroundColor = pair.Color;

        sr.sprite = pair.Background;
    }
}
