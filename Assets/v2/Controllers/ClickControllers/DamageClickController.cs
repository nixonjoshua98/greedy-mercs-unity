using UnityEngine;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class DamageClickController : AbstractClickController
    {
        GameManager GameManager;
        GM.UI.IDamageNumberManager DamageNumberManager;

        // Bounds
        Vector2 topLeftViewportPosition;
        Vector2 btmRightViewportPosition;

        void Start()
        {
            GameManager = this.GetComponentInScene<GameManager>();
            DamageNumberManager = this.GetComponentInScene<GM.UI.IDamageNumberManager>();

            Vector3[] corners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(corners);

            topLeftViewportPosition = Camera.main.ScreenToViewportPoint(corners[1]);
            btmRightViewportPosition = Camera.main.ScreenToViewportPoint(corners[3]);
        }

        protected override void OnClick(Vector2 screenPos)
        {
            Vector3 viewportClickPosition = Camera.main.ScreenToViewportPoint(screenPos);

            BigDouble damage = App.Cache.TotalTapDamage;

            if (CollisionCheck(viewportClickPosition) && GameManager.DealDamageToTarget(damage, showDamageNumber: false))
            {
                DamageNumberManager.Spawn(Camera.main.ViewportToWorldPoint(viewportClickPosition), Format.Number(damage));
            }
        }

        bool CollisionCheck(Vector3 pos)
        {
            return (pos.x > topLeftViewportPosition.x && pos.x < btmRightViewportPosition.x) && (pos.y < topLeftViewportPosition.y && pos.y > btmRightViewportPosition.y);
        }
    }
}
