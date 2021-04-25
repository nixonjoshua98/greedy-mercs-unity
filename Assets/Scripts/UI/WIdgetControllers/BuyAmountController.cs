using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class BuyAmountController : MonoBehaviour
    {
        [SerializeField] Button[] Buttons;

        [SerializeField] int[] amounts;

        Image[] Images;

        [HideInInspector] public int BuyAmount;

        Color[] originalColours;

        void Awake()
        {
            Images = new Image[Buttons.Length];

            originalColours = new Color[Buttons.Length];

            for (int i = 0; i < Buttons.Length; ++i)
            {
                int temp = i;

                Button current = Buttons[i];

                current.onClick.AddListener(delegate { OnChange(temp); });

                Images[i] = current.GetComponent<Image>();
                originalColours[i] = current.GetComponent<Image>().color;
            }
        }
        void Start()
        {
            OnChange(0);
        }

        public void OnChange(int index)
        {
            BuyAmount = amounts[index];

            for (int i = 0; i < Buttons.Length; ++i)
            {
                Images[i].color = (i == index) ? new Color(0, 0.5f, 0, 255) : originalColours[i];
            }
        }
    }
}