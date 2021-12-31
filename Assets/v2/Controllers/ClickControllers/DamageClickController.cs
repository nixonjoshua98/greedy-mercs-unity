using GM.Common;
using GM.Targets;
using GM.UI;
using UnityEngine;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class DamageClickController : AbstractClickController
    {
        [SerializeField] ObjectPool DamageTextPool;

        Vector3[] RectTransformCorners;

        void Start()
        {
            RectTransformCorners = new Vector3[4];

            GetComponent<RectTransform>().GetWorldCorners(RectTransformCorners);
        }

        protected override void OnClick(Vector3 screenPos)
        {
            Vector3 worldPos = PositionRelativeToScreenWorldPosition(screenPos);

            if (CollisionCheck(worldPos))
            {
                bool hasTarget = GameManager.Instance.TryGetEnemy(out Target trgt);

                if (hasTarget)
                {
                    BigDouble dmg = App.Cache.TotalTapDamage;

                    BigDouble dmgDealt = trgt.Health.TakeDamage(dmg);

                    if (dmgDealt > 0)
                    {
                        TextPopup popup = DamageTextPool.Spawn<TextPopup>();

                        popup.Set(dmg, GM.Common.Colors.Red, screenPos);
                    }                 
                }
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
