using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneTransition = GM.Scene.SceneTransition;


namespace GM
{
    public class PrestigePanel : Core.GMMonoBehaviour
    {
        [SerializeField] Text prestigePointText;
        [SerializeField] Button prestigeButton;

        [Header("Objects")]
        [SerializeField] GameObject sceneTransitionObject;

        void FixedUpdate()
        {
            prestigePointText.text = Format.Number(App.Cache.PrestigePointsAtStage(App.Data.GameState.Stage));
        }


        public void Prestige()
        {
            if (App.Data.GameState.Stage >= Common.Constants.MIN_PRESTIGE_STAGE)
            {
                prestigeButton.interactable = false;

                RequestPrestige();
            }
        }


        void RequestPrestige()
        {
            App.Data.Prestige((success) =>
            {
                prestigeButton.interactable = !success;

                if (success)
                {
                    SceneTransition transition = InstantiateUI<SceneTransition>(sceneTransitionObject);

                    transition.E_OnFinished.AddListener(() =>
                    {
                        SceneManager.LoadSceneAsync("GameScene");
                    });
                }
            });
        }
    }
}