using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Coffee.UIEffects;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;

    struct ItemSlotColour
    {
        public Color FrameColour;
        public Color BackgroundColour { get { return FrameColour / 2; } }
    }

    public class ArmouryItemSlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image itemImage;
        [SerializeField] Image frameImage;
        [SerializeField] Image backgroundImage;
        [Space]
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] GameObject lockedPanel;
        [SerializeField] GameObject[] stars;

        [Header("Effects")]
        [SerializeField] UIShiny shinyEffect;

        ArmouryItemSO scriptableItem;

        ArmouryWeaponState state { get { return GameState.Armoury.GetWeapon(scriptableItem.ItemID); } }

        public void Init(ArmouryItemSO weapon, bool hideLockedPanel = false)
        {
            scriptableItem = weapon;

            UpdateUI();
            UpdateStarRating();
            UpdateShinyEffect(hideLockedPanel: hideLockedPanel);
            ToggleLockedText(hideLockedPanel);
        }

        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(e);
        }

        void UpdateUI()
        {
            Color[] colors = new Color[3] { Color.black, Color.green, Color.blue };

            ItemSlotColour slotColour = new ItemSlotColour() { FrameColour = colors[scriptableItem.itemTier - 1] };

            itemImage.sprite = scriptableItem.icon;

            frameImage.color        = slotColour.FrameColour;
            backgroundImage.color   = slotColour.BackgroundColour;
        }

        void UpdateStarRating()
        {
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i].SetActive(i <= scriptableItem.itemTier - 1);
            }
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
