using UnityEngine;

namespace GM.Controllers
{
    public abstract class AbstractClickController : Core.GMMonoBehaviour
    {
        private void Update()
        {
#if UNITY_EDITOR
            Editor_MouseEvents();
#else
            MouseEvents();
#endif
        }
        protected abstract void OnClick(Vector3 pos);

        void Editor_MouseEvents()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
            }
        }

        void MouseEvents()
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch t = Input.GetTouch(i);

                if (t.phase == TouchPhase.Began)
                {
                    OnClick(new Vector3(t.position.x, t.position.y));
                }
            }
        }
    }
}