using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Background
{
    public class CameraController : MonoBehaviour
    {
        void LateUpdate()
        {
            List<Vector3> unitPositions = MercManager.Instance.UnitPositions;

            if (unitPositions.Count > 0)
            {
                UpdateCamera(unitPositions.OrderByDescending(ele => ele.x).First());
            }
        }


        void UpdateCamera(Vector3 pos)
        {
            Vector3 temp = transform.position;

            if (pos.x > temp.x)
            {
                transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
            }
        }
    }
}
