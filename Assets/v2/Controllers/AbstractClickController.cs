using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace GM.Controllers
{
    public abstract class AbstractClickController : Core.GMMonoBehaviour
    {
        public int MaxClicksPerSeconds = 20;
        private Stopwatch ClickStopWatch;
        private float ClickIntervalMilliseconds;

        private void Awake()
        {
            ClickStopWatch = Stopwatch.StartNew();
            ClickIntervalMilliseconds = 1.0f / MaxClicksPerSeconds;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Editor_MouseEvents();
#else
            Mobile_TouchEvents();
#endif
        }

        private void Editor_MouseEvents()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttemptClick(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
        }

        private void Mobile_TouchEvents()
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch t = Input.GetTouch(i);

                if (t.phase == TouchPhase.Began)
                {
                    AttemptClick(new Vector2(t.position.x, t.position.y));
                }
            }
        }

        private void AttemptClick(Vector2 screenPosition)
        {
            float msSinceLastClick = ClickStopWatch.ElapsedMilliseconds;

            if (msSinceLastClick >= ClickIntervalMilliseconds)
            {
                ClickStopWatch.Restart();

                OnClick(screenPosition);
            }
        }

        protected abstract void OnClick(Vector2 pos);
    }
}