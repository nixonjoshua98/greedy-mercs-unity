using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuyAmountController : MonoBehaviour
{
    [SerializeField] Button[] Buttons;

    [SerializeField] int[] amounts;

    Image[] Images;

    [HideInInspector] public int BuyAmount;

    void Awake()
    {
        BuyAmount = 1;

        Images = new Image[Buttons.Length];

        for (int i = 0; i < Buttons.Length; ++i)
        {
            int temp = i;

            Button current = Buttons[i];

            current.onClick.AddListener(delegate { OnChange(temp); });

            Images[i] = current.GetComponent<Image>();
        }
    }

    public void OnChange(int index)
    {
        BuyAmount = amounts[index];

        for (int i = 0; i < Buttons.Length; ++i)
        {
            Images[i].color = (i == index) ? new Color(0, 1, 0, 190.0f / 255) : new Color(1, 1, 1, 190.0f / 255);
        }
    }
}
