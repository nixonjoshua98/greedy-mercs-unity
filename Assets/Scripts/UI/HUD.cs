using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] Text StageText;
        [SerializeField] Text GoldText;

        void FixedUpdate()
        {
            CurrentStageState state = GameManager.Instance.GetState();

            GoldText.text = Utils.Format.FormatNumber(GameState.Player.gold);

            StageText.text = "Stage " + state.currentStage.ToString();
        }
    }
}