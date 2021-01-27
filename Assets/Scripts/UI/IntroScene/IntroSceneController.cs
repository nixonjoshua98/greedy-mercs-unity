using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GreedyMercs.IntroScene
{
    public class IntroSceneController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text[] introText;
        [SerializeField] Button continueButton;

        void Awake()
        {
            foreach (Text t in introText)
                t.color = new Color(t.color.r, t.color.g, t.color.b, 0.0f);

            continueButton.gameObject.SetActive(false);
        }

        IEnumerator Start()
        {
            foreach (Text t in introText)
            {
                yield return FadeInText(t, 2.0f);
            }

            continueButton.gameObject.SetActive(true);
        }

        IEnumerator FadeInText(Text t, float duration)
        {
            float progress = 0;

            while (progress < 1.0f)
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, progress);

                progress += (Time.deltaTime / duration);

                yield return new WaitForEndOfFrame();
            }
        }

        public void StartGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
