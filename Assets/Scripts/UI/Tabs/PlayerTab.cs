
using UnityEngine;


public class PlayerTab : MonoBehaviour
{
    static PlayerTab Instance = null;

    [SerializeField] BuyAmountController buyAmount;

    public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

    void Awake()
    {
        Instance = this;
    }
}
