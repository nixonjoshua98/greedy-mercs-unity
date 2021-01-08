
using UnityEngine;
using UnityEngine.EventSystems;

public class TapController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] ParticleSystem ps;

    public void Awake()
    {
        //InvokeRepeating("DoClick", 0.1f, 0.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ps.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 1.0f));

        ps.Play();

        DoClick();
    }

    void DoClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
