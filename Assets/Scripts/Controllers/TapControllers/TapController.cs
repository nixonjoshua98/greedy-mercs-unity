
using UnityEngine;
using UnityEngine.EventSystems;

namespace GM
{
    public abstract class TapController : MonoBehaviour, IPointerDownHandler
    {
        [Header("Components")]
        [SerializeField] Camera mainCamera;
        [SerializeField] ParticleSystem ps;

        [Header("Properties")]
        [SerializeField] int maxClicksPerSecond;

        [Space]

        float clickDelay;

        float lastClickTime = 0;

        Camera MainCamera { get { return mainCamera ? mainCamera : Camera.main; } }

        void Awake()
        {
            clickDelay = 1.0f / maxClicksPerSecond;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            float clickTimeNow = Time.time;

            if (clickTimeNow - lastClickTime >= clickDelay)
            {
                lastClickTime = clickTimeNow;

                Vector3 worldPos = MainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 1.0f));

                OnClick(new Vector3(worldPos.x, worldPos.y, 10.0f));
            }
        }

        protected void ActivateParticles(Vector3 worldPos)
        {
            ps.gameObject.transform.position = worldPos;

            ps.Play();
        }

        protected abstract void OnClick(Vector3 worldPos);
    }
}