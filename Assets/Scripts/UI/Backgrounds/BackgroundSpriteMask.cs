

using UnityEngine;

namespace GM.Background
{
    public class BackgroundSpriteMask : MonoBehaviour
    {
        public RectTransform rt;

        void Awake()
        {
            ResizeSpriteMask();
        }

        void ResizeSpriteMask()
        {
            Vector3[] corners = new Vector3[4];

            rt.GetWorldCorners(corners);

            Vector3 topLeft = corners[1];
            Vector3 btmRight = corners[3];

            Vector3 temp = transform.position;

            temp.y = (topLeft.y + btmRight.y) / 2.0f;

            transform.position = temp;

            transform.localScale = new Vector3(
                Mathf.Abs(btmRight.x - topLeft.x),
                Mathf.Abs(btmRight.y - topLeft.y)
                );
        }
    }
}
