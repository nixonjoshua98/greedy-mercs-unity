
using UnityEngine;

public class TapController : MonoBehaviour
{
    void Awake()
    {
        InvokeRepeating("DoClick", 0.0f, 0.2f);
    }

    public void OnClick()
    {
        DoClick();
    }

    void DoClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
