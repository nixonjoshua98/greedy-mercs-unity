using UnityEngine;

namespace SRC.Units.Controllers
{
    public class UnitShadow : MonoBehaviour
    {
        [SerializeField] Transform Anchor;

        float InitialAnchorDistance;
        Vector2 InitialScale;

        void Awake()
        {
            InitialScale = transform.localScale;
            InitialAnchorDistance = Mathf.Abs(Anchor.position.y - transform.position.y);
        }

        void FixedUpdate()
        {
            float distance = Mathf.Min(InitialAnchorDistance, Mathf.Abs(Anchor.position.y - transform.position.y));

            // Percentage between 0 - InitialDistance
            float percent = 1 - (distance / InitialAnchorDistance);

            // Scale multiplier (multi = 1 when distance is initial, multi = 1.5 when distance is zero)
            float scaleMulti = 1 + (percent * 1.5f);

            transform.localScale = new Vector3(InitialScale.x * scaleMulti, transform.localScale.y);
        }
    }
}
