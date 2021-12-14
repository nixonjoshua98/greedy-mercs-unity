using UnityEngine;
using UnityEngine.Events;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class DamageClickController : AbstractClickController
    {
        public UnityEvent<Vector2> E_OnClick { get; private set; } = new UnityEvent<Vector2>();

        Vector3[] RectTransformCorners;

        void Start()
        {
            RectTransformCorners = new Vector3[4];

            GetComponent<RectTransform>().GetWorldCorners(RectTransformCorners);
        }

        protected override void OnClick(Vector3 pos)
        {
            pos = PositionRelativeToScreenWorldPosition(pos);

            if (CollisionCheck(pos))
            {
                E_OnClick.Invoke(pos);
            }
        }

        Vector3 PositionRelativeToScreenWorldPosition(Vector3 pos)
        {
            // Camera moves in the screen space, so we need to 'reset' the click position so we can use the
            // original rect corners we fetch at the beginning
            return Camera.main.ScreenToWorldPoint(pos) - Camera.main.transform.position;
        }

        bool CollisionCheck(Vector3 pos)
        {
            Vector3 topLeft  = RectTransformCorners[1];
            Vector3 btmRight = RectTransformCorners[3];

            return (pos.x > topLeft.x && pos.x < btmRight.x) && (pos.y < topLeft.y && pos.y > btmRight.y);
        }
    }
}
