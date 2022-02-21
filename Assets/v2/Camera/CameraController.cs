using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GM.Mercs;

namespace GM.CameraControllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] MercSquadController MercSquad;
        [Space]
        public float Speed = 2.5f;

        void LateUpdate()
        {
            List<float> xPositions = new List<float>();

            var mercPositions = MercSquad.MercPositions;

            if (mercPositions.Count > 0)
            {
                xPositions.Add(mercPositions.Average(pos => pos.x));
            }

            if (GameManager.Instance.Enemies.Count > 0)
            {
                float xEnemiesPos = GameManager.Instance.Enemies.Select(x => x.transform.position).Average(pos => pos.x);
                xPositions.Add(xEnemiesPos);
            }

            if (xPositions.Count > 0)
            {
                UpdateCamera(Mathf.Max(0, xPositions.Average()));
            }
        }

        void UpdateCamera(float xPos)
        {
            xPos = MathUtils.MoveTo(transform.position.x, xPos, Speed * Time.unscaledDeltaTime);

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
}
