using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.UI;

    using GreedyMercs.Armoury.Data;

    struct ItemSlotColour
    {
        public Color FrameColour;
        public Color BackgroundColour { get { Color bg = FrameColour / 2; bg.a = 1.0f; return bg; } }
    }

    public class ArmouryItem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image itemImage;
        [SerializeField] Image frameImage;
        [SerializeField] Image backgroundImage;
        [Space]
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        ArmouryItemSO scriptableItem;

        public void Init(ArmouryItemSO weapon)
        {
            scriptableItem = weapon;

            UpdateUI();

            stars.SetRating(scriptableItem.starRating);
        }

        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(e);
        }

        void UpdateUI()
        {
            Color[] colors = new Color[3] { Color.gray, Color.green, Color.blue };

            ItemSlotColour slotColour = new ItemSlotColour() { FrameColour = colors[scriptableItem.starRating - 1] };

            itemImage.sprite = scriptableItem.icon;

            frameImage.color        = slotColour.FrameColour;
            backgroundImage.color   = slotColour.BackgroundColour;
        }
    }
}
