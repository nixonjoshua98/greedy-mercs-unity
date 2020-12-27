
using UnityEngine;

public class TapController : MonoBehaviour
{
    public void OnClick()
    {
        DoClick();
    }

    void DoClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
