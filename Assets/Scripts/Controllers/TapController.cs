
using UnityEngine;

public class TapController : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());
    }
}
