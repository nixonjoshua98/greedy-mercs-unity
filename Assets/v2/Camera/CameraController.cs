using System.Linq;
using UnityEngine;
using GM.Targets;

namespace GM.CameraControllers
{
    public class CameraController : MonoBehaviour
    {
        void LateUpdate()
        {
            var mercs = MercManager.Instance.Mercs;

            if (mercs.Count > 0)
            {
                float xAvg = mercs.Average(x => x.Position.x);

                if (GameManager.Instance.TryGetStageBoss(out UnitTarget boss))
                {
                    xAvg = (boss.Position.x + xAvg) / 2;
                }

                UpdateCamera(Mathf.Max(0, xAvg));
            }
        }


        void UpdateCamera(float xPos)
        {
            xPos = MathUtils.MoveTo(transform.position.x, xPos, 1.75f * Time.unscaledDeltaTime);

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
}
