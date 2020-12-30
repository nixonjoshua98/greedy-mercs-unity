
using UnityEngine;

public class TapController : MonoBehaviour
{
    public void Awake()
    {
        InvokeRepeating("DoClick", 0.1f, 0.1f);
    }

    public void OnClick()
    {

    }

    void DoClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
