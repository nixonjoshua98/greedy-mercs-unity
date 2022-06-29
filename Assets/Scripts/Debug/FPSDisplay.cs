using UnityEngine;

namespace GM
{
    public class FPSDisplay : MonoBehaviour
    {
        private float deltaTime = 0.0f;

        void Awake()
        {
            if (!Application.isEditor)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(w - 512, (-h) + 512, w, h);

            style.alignment = TextAnchor.LowerLeft;
            style.normal.textColor = Color.white;
            style.fontSize = h * 2 / 100;

            float fps = 1.0f / deltaTime;
            float ms = deltaTime * 1000.0f;

            string text = string.Format("{0:0.0} ms ({1:0.} fps)", ms, fps);

            GUI.Label(rect, text, style);
        }
    }
}