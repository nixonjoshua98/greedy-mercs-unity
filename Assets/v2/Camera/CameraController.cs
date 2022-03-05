using GM.Mercs;
using GM.Units.Mercs;
using UnityEngine;

namespace GM
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float MoveSpeed = 10.0f;

        ISquadController MercSquad;

        void Awake()
        {
            MercSquad = this.GetComponentInScene<ISquadController>();
        }


        void FixedUpdate()
        {
            if (MercSquad.TryGetFrontUnitQueue(out MercBaseClass unit))
            {
                SetCameraPosition(unit.Avatar.Bounds.max.x);
            }
        }

        void SetCameraPosition(float xPos)
        {
            Vector3 to = new Vector3(Mathf.Max(transform.position.x, xPos), transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, to, MoveSpeed * Time.fixedUnscaledDeltaTime);
        }
    }
}
