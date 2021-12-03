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
            CurrentStageState state = GameManager.Instance.State();

            prestigePointText.text = Format.Number(App.Cache.PrestigePointsForStage(state.Stage));
        }


        public void Prestige()
        {
            CurrentStageState state = GameManager.Instance.State();

            if (state.Stage >= Common.Constants.MIN_PRESTIGE_STAGE)
            {
                prestigeButton.interactable = false;

                RequestPrestige();
            }
        }


        void RequestPrestige()
        {
            App.Data.Prestige( (success) =>
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