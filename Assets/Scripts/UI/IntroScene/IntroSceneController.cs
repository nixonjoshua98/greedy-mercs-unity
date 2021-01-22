using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GreedyMercs.IntroScene
{
    public class IntroSceneController : MonoBehaviour
    {
        [SerializeField] string finalTextString;

        [Header("Components")]
        [SerializeField] Text text;
        [SerializeField] Button continueButton;

        void Awake()
        {
            continueButton.gameObject.SetActive(false);
        }

        IEnumerator Start()
        {
            float progress = 0;

            while (progress < 1.0f)
            {
                text.text = finalTextString.Substring(0, Mathf.CeilToInt(finalTextString.Length * progress));

                progress += (Time.deltaTime / 10.0f);

                yield return new WaitForEndOfFrame();
            }

            continueButton.gameObject.SetActive(true);
        }

        public void StartGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
