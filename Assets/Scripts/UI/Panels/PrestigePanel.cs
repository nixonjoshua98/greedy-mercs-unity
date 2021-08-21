using System.Numerics;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;
using GM.Data;
using UnityEngine.UI;


namespace GM
{
    public class PrestigePanel : MonoBehaviour
    {
        [SerializeField] Text prestigePointText;
        [SerializeField] Button prestigeButton;

        void FixedUpdate()
        {
            CurrentStageState state = GameManager.Instance.State();

            prestigePointText.text = FormatString.Number(StatsCache.GetPrestigePoints(state.Stage));
        }


        public void Prestige()
        {
            CurrentStageState state = GameManager.Instance.State();

            if (state.Stage >= StaticData.MIN_PRESTIGE_STAGE)
            {
                prestigeButton.interactable = false;

                RequestPrestige();
            }
        }


        void RequestPrestige()
        {
            CurrentStageState state = GameManager.Instance.State();

            JSONNode node = new JSONObject();

            node.Add("prestigeStage", state.Stage);

            UserData.Get.Prestige(node, (success, resp) =>
            {
                if (success)
                {
                    SceneManager.LoadSceneAsync("GameScene");
                }
                else
                {
                    prestigeButton.interactable = true;
                }
            });
        }
    }
}