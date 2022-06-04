using GM.Mercs;
using UnityEngine;

namespace GM
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed = 10.0f;
        private MercSquadController MercSquad;

        private void Awake()
        {
            MercSquad = this.GetComponentInScene<MercSquadController>();
        }

        private void FixedUpdate()
        {
            if (MercSquad.Count > 0)
            {
                var unit = MercSquad.First();

                SetCameraPosition(unit.Avatar.Bounds.max.x);
            }
        }

        private void SetCameraPosition(float xPos)
        {
            Vector3 to = new Vector3(Mathf.Max(transform.position.x, xPos), transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, to, MoveSpeed * Time.fixedUnscaledDeltaTime);
        }
    }
}
