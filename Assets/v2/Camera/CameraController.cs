using System.Linq;

using UnityEngine;

namespace GM.CameraControllers
{
    public class CameraController : MonoBehaviour
    {
        void LateUpdate()
        {
            var mercs = GameManager.Instance.Mercs;

            if (mercs.Count > 0)
            {
                float xPos = mercs.Average(x => x.Position.x);

                UpdateCamera(xPos);
            }
        }


        void UpdateCamera(float xPos)
        {
            xPos = Mathf.Max(0, xPos);
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
}
