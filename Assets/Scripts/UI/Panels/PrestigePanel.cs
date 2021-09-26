using System.Numerics;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;
using GM.Core;
using UnityEngine.UI;
using SceneTransition = GM.Scene.SceneTransition;


namespace GM
{
    public class PrestigePanel : MonoBehaviour
    {
        [SerializeField] Text prestigePointText;
        [SerializeField] Button prestigeButton;

        [Header("Objects")]
        [SerializeField] GameObject sceneTransitionObject;

        void FixedUpdate()
        {
            CurrentStageState state = GameManager.Instance.State();

            prestigePointText.text = FormatString.Number(StatsCache.GetPrestigePoints(state.Stage));
        }


        public void Prestige()
        {
            CurrentStageState state = GameManager.Instance.State();

            if (state.Stage >= global::Constants.MIN_PRESTIGE_STAGE)
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
                prestigeButton.interactable = !success;

                if (success)
                {
                    SceneTransition transition = CanvasUtils.Instantiate<SceneTransition>(sceneTransitionObject);

                    transition.E_OnFinished.AddListener(() =>
                    {
                        SceneManager.LoadSceneAsync("InitScene", LoadSceneMode.Additive);
                    });
                }
            });
        }
    }
}