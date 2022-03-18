using UnityEngine;

namespace GM
{
    public class ClickObject : MonoBehaviour
    {
        public ParticleSystem PS;

        Vector3? TargetScreenPosition = null;

        public void SetScreenPosition(Vector3 pos)
        {
            TargetScreenPosition = (Vector3?)pos;
        }

        private void OnDisable()
        {
            TargetScreenPosition = null;
        }

        private void FixedUpdate()
        {
            if (TargetScreenPosition.HasValue)
            {
                // Keep the object at the same screen position (even if camera moves)
                Vector3 pos = Camera.main.ScreenToWorldPoint((Vector3)TargetScreenPosition);

                transform.position = new Vector3(pos.x, pos.y);
            }
        }
    }
}
