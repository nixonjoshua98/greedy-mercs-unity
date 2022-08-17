using System.Collections;
using UnityEngine;

namespace SRC.CameraControllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera Camera;

        public void MoveCamera(Vector3 vec, float duration)
        {
            StartCoroutine(MoveCameraEnumerator(vec, duration));
        }

        public IEnumerator MoveCameraEnumerator(Vector3 vec, float duration)
        {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = initialPosition + vec;

            yield return this.Lerp(0, 1, duration, (value) =>
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, value);
            });
        }

        public Bounds Bounds => Camera.Bounds();
    }
}
