
using UnityEngine;
using UnityEngine.EventSystems;

public class TapController : MonoBehaviour, IPointerDownHandler
{
    const float CLICK_DELAY = 1.0f / 20.0f;

    [SerializeField] ParticleSystem ps;

    float lastClickTime = 0;

    public void Awake()
    {
        //InvokeRepeating("DoClick", 0.1f, 0.1f);
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
    }
}
