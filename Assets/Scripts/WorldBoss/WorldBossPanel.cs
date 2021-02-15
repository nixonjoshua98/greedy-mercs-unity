using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreedyMercs.WorldBoss.UI
{
    public class WorldBossPanel : MonoBehaviour
    {
        void SVR_StartFight() => FightServerCallback(200, "");

        Canvas gameSceneCanvas;

        void Awake()
        {
            gameSceneCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        }

        IEnumerator StartWorldBossScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("WorldBossScene", LoadSceneMode.Additive);

            while (!operation.isDone)
                yield return null;

            FocusWorldBossScene();

            Destroy(gameObject);
        }

        void FocusWorldBossScene()
        {
            gameSceneCanvas.enabled = false;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("WorldBossScene"));
        }

        // === Button Callbacks ===
        public void OnFightButtonClicked()
        {
            Invoke("SVR_StartFight", 0.5f);
        }

        // === Server Callbacks ===
        void FightServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                StartCoroutine(StartWorldBossScene());
            }
        }
    }
}