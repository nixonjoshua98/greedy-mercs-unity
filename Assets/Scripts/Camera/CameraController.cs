using GM.Units;
using UnityEngine;

namespace GM
{
    public class CameraController : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float MoveSpeed = 10.0f;

        [Header("References")]
        [SerializeField] private EnemyUnitCollection EnemyUnits;

        private void FixedUpdate()
        {
            if (EnemyUnits.Count > 0)
            {
                var unit = EnemyUnits.First();

                SetCameraPosition(unit.transform.position.x);
            }
        }

        private void SetCameraPosition(float xPos)
        {
            Vector3 to = new(xPos, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, to, MoveSpeed * Time.fixedUnscaledDeltaTime);
        }
    }
}
