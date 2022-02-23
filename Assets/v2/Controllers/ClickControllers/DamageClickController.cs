using UnityEngine;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class DamageClickController : AbstractClickController
    {
        IUnitManager UnitManager;
        GM.UI.IDamageNumberManager DamageNumberManager;

        // Bounds
        Vector2 topLeftViewportPosition;
        Vector2 btmRightViewportPosition;

        void Start()
        {
            DamageNumberManager = this.GetComponentInScene<GM.UI.IDamageNumberManager>();
            UnitManager = this.GetComponentInScene<IUnitManager>();

            Vector3[] corners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(corners);

            topLeftViewportPosition = Camera.main.ScreenToViewportPoint(corners[1]);
            btmRightViewportPosition = Camera.main.ScreenToViewportPoint(corners[3]);
        }

        protected override void OnClick(Vector3 screenPos)
        {
            Vector3 viewportClickPosition = Camera.main.ScreenToViewportPoint(screenPos);

            if (CollisionCheck(viewportClickPosition) && UnitManager.TryGetEnemyUnit(out GM.Units.UnitBaseClass unit))
            {
                GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

                BigDouble dmg = App.Cache.TotalTapDamage;

                health.TakeDamage(dmg);

                DamageNumberManager.Spawn(Camera.main.ViewportToWorldPoint(viewportClickPosition), Format.Number(dmg));
            }
        }

        bool CollisionCheck(Vector3 pos)
        {
            return (pos.x > topLeftViewportPosition.x && pos.x < btmRightViewportPosition.x) && (pos.y < topLeftViewportPosition.y && pos.y > btmRightViewportPosition.y);
        }
    }
}
