using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class Background : MonoBehaviour
    {
        /* ===
         * This class handles the background transitions for the game, transitions occur every 25 stages
         * 
         * Note
         *      Feedback told me that it looks better without the transition
         * ===
         */

        [SerializeField] Sprite[] backgrounds;

        void Awake()
        {
            Events.OnNewStageStarted.AddListener(OnNewStageStarted);
        }

        void Start()
        {
            ChangeBackground();
        }

        void OnNewStageStarted()
        {
            if ((GameState.Stage.stage - 1) % 25 == 0)
            {
                ChangeBackground();
            }
        }

        void ChangeBackground()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            sr.sprite = backgrounds[Mathf.FloorToInt(GameState.Stage.stage / 25) % backgrounds.Length];
        }
    }
}