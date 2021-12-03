using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class DamageClickController : MonoBehaviour
    {
        public UnityEvent<Vector2> E_OnClick { get; private set; } = new UnityEvent<Vector2>();

        Vector3[] RectTransformCorners;

        private void Start()
        {
            RectTransformCorners = new Vector3[4];

            GetComponent<RectTransform>().GetWorldCorners(RectTransformCorners);
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (CollisionCheck(pos))
                {
                    E_OnClick.Invoke(pos);
                }
            }
#else
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch t = Input.GetTouch(i);

                if (t.phase == TouchPhase.Began)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);

                    if (CollisionCheck(pos))
                    {
                        E_OnClick.Invoke(pos);
                    }
                }
            }
#endif
        }

        bool CollisionCheck(Vector3 pos)
        {
            Vector3 topLeft  = RectTransformCorners[1];
            Vector3 btmRight = RectTransformCorners[3];

            return (pos.x > topLeft.x && pos.x < btmRight.x) && (pos.y < topLeft.y && pos.y > btmRight.y);
        }
    }
}
