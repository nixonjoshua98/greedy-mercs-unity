using GM.Mercs;
using UnityEngine;

namespace GM
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed = 10.0f;
        private ISquadController MercSquad;

        private void Awake()
        {
            MercSquad = this.GetComponentInScene<ISquadController>();
        }

        private void FixedUpdate()
        {
            if (MercSquad.TryGetUnit(out MercBaseClass unit))
            {
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
