
using UnityEngine;

public class TapController : MonoBehaviour
{
    public void OnClick()
    {
        for (int i = 0; i < 10; ++i)
            DoClick();
    }

    void DoClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
