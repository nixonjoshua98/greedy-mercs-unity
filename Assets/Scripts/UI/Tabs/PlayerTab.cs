
using UnityEngine;


public class PlayerTab : MonoBehaviour
{
    static PlayerTab Instance = null;

    [SerializeField] BuyAmountController buyAmount;
    [Space]
    [SerializeField] GameObject PlayerStatsPanel;

    public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

    void Awake()
    {
        Instance = this;
    }

    // === Button Callbacks ===

    public void OnShowStats()
    {
        Utils.UI.Instantiate(PlayerStatsPanel, Vector3.zero);
    }
}
