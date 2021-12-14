using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Controllers
{
    public abstract class AbstractClickController : MonoBehaviour
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
                OnClick(Input.mousePosition);
            }
        }

        void MouseEvents()
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch t = Input.GetTouch(i);

                if (t.phase == TouchPhase.Began)
                {
                    OnClick(t.position);
                }
            }
        }
    }
}