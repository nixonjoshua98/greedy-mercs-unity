
using UnityEngine;
using UnityEngine.EventSystems;

namespace GreedyMercs
{
    public class TapController : MonoBehaviour, IPointerDownHandler
    {
        const float CLICK_DELAY = 1.0f / 20.0f;

        [SerializeField] ParticleSystem ps;

        float lastClickTime = 0;
        float lastAutoTapTime = 0;

        public void Awake()
        {
            lastAutoTapTime = Time.time;

            InvokeRepeating("AutoTap", 1.0f, 1.0f);
        }

        void AutoTap()
        {
            float secsSinceAuto = Time.time - lastAutoTapTime;

            BigDouble dmg = StatsCache.GoldUpgrades.AutoTapDamage() * Mathf.Min(1, secsSinceAuto);

            if (dmg >= 1)
            {
                GameManager.TryDealDamageToEnemy(StatsCache.GoldUpgrades.AutoTapDamage() * Mathf.Min(1, secsSinceAuto));

                lastAutoTapTime = Time.time;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            float clickTimeNow = Time.time;

            if (clickTimeNow - lastClickTime >= CLICK_DELAY)
            {
                lastClickTime = clickTimeNow;

                ps.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 1.0f));

                ps.Play();

                DoClick();
            }
        }

        void DoClick()
        {
            GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());

            Events.OnPlayerTap.Invoke();
        }
    }
}