
using UnityEngine;
using UnityEngine.UI;


public class PlayerTab : MonoBehaviour
{
    [SerializeField] Image[] BuyAmountImages;

    // = Static Fields ==
    public static int BuyAmount = 1;
    // == Static Fields ==

    // === Button Callbacks === 

    void Awake()
    {
        BuyAmount = 1;
    }

    public void OnBuyAmountChange(int newBuyAmount)
    {
        BuyAmountImages[(int)Mathf.Log10(BuyAmount)].color      = new Color(1, 1, 1, 190.0f / 255);
        BuyAmountImages[(int)Mathf.Log10(newBuyAmount)].color   = new Color(0, 1, 0, 190.0f / 255);

        BuyAmount = newBuyAmount;
    }
}
