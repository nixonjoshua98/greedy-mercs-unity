using System.Linq;
using UnityEngine;
using GM.Targets;
using System.Collections.Generic;

namespace GM.CameraControllers
{
    public class CameraController : MonoBehaviour
    {
        void LateUpdate()
        {
            List<Vector3> positions = new List<Vector3>();

            if (GameManager.Instance.Mercs.Count > 0)
                positions = GameManager.Instance.Mercs.Select(merc => merc.Position).ToList();

            else if (GameManager.Instance.Enemies.Count > 0)
                positions = GameManager.Instance.Enemies.Select(x => x.Position).ToList();

            if (positions.Count > 0)
            {
                float xAvg = positions.Average(x => x.x);

                if (GameManager.Instance.TryGetStageBoss(out UnitTarget boss))
                {
                    xAvg = (boss.Position.x + xAvg) / 2;
                }

                UpdateCamera(Mathf.Max(0, xAvg));
            }
        }


        void UpdateCamera(float xPos)
        {
            xPos = MathUtils.MoveTo(transform.position.x, xPos, 2.0f * Time.unscaledDeltaTime);

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
}
