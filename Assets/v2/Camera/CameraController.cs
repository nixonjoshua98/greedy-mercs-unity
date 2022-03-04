using GM.Mercs;
using GM.Units;
using UnityEngine;

namespace GM
{
    public class CameraController : MonoBehaviour
    {
        ISquadController MercSquad;

        void Awake()
        {
            MercSquad = this.GetComponentInScene<ISquadController>();
        }


        void LateUpdate()
        {
            if (MercSquad.TryGetFrontUnitQueue(out UnitBaseClass unit))
            {
                SetCameraPosition(unit.Avatar.Bounds.max.x);
            }
        }

        void SetCameraPosition(float xPos)
        {
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
}
