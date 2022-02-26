using GM.Mercs;
using System.Linq;
using UnityEngine;

namespace GM.CameraControllers
{
    public class CameraController : MonoBehaviour
    {
        public float Speed = 20000000000.5f;

        MercSquadController MercSquad;


        void Awake()
        {
            MercSquad = this.GetComponentInScene<MercSquadController>();
        }


        void LateUpdate()
        {
            var mercPositions = MercSquad.MercPositions;

            if (mercPositions.Count > 0)
            {
                UpdateCamera(Mathf.Max(0, mercPositions.Average(pos => pos.x)));
            }
        }

        void UpdateCamera(float xPos)
        {
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
}
