using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Coffee.UIEffects;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.UI;

    using GreedyMercs.Armoury.Data;

    struct ItemSlotColour
    {
        public Color FrameColour;
        public Color BackgroundColour { get { return FrameColour / 2; } }
    }

    public class ArmouryItemSlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text slotText;
        [Space]
        [SerializeField] Image itemImage;
        [SerializeField] Image frameImage;
        [SerializeField] Image backgroundImage;
        [Space]
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;
        [SerializeField] GameObject lockedPanel;

        [Header("Effects")]
        [SerializeField] UIShiny shinyEffect;

        ArmouryItemSO scriptableItem;

        ArmouryWeaponState state { get { return GameState.Armoury.GetWeapon(scriptableItem.ItemID); } }

        public void Init(ArmouryItemSO weapon, bool hideLockedPanel = false)
        {
            scriptableItem = weapon;

            UpdateUI();

            stars.UpdateRating(scriptableItem.starRating);
            
            UpdateShinyEffect(hideLockedPanel: hideLockedPanel);
            ToggleLockedText(hideLockedPanel);

            slotText.text = scriptableItem.name;
        }

        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(e);
        }

        void UpdateUI()
        {
            Color[] colors = new Color[3] { Color.black, Color.green, Color.blue };

            ItemSlotColour slotColour = new ItemSlotColour() { FrameColour = colors[scriptableItem.starRating - 1] };

            itemImage.sprite = scriptableItem.icon;

            frameImage.color        = slotColour.FrameColour;
            backgroundImage.color   = slotColour.BackgroundColour;
        }

        void ToggleLockedText(bool hideLockedPanel)
        {
            bool isUnlocked = (state.level + state.evoLevel + state.owned) > 0;

            lockedPanel.SetActive(!isUnlocked && !hideLockedPanel);

            button.interactable = !lockedPanel.activeInHierarchy;
        }

        void UpdateShinyEffect(bool hideLockedPanel)
        {
            if (state.owned > 0 || hideLockedPanel)
                shinyEffect.Play(reset: false);
            else
                shinyEffect.Stop(reset: true);
        }
    }
}
