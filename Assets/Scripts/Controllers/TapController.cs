
using UnityEngine;

public class TapController : MonoBehaviour
{
    public void OnClick()
    {
        DoClick();
        DoClick();
        DoClick();
        DoClick();
        DoClick();
        DoClick();
        DoClick();
        DoClick();
        DoClick();
        DoClick();
    }

    void DoClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
