
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.UI;

    using GreedyMercs.Armoury.Data;

    public class ArmouryItem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image itemImage;
        [Space]
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        ArmouryItemSO scriptableItem;

        public void Init(ArmouryItemSO weapon)
        {
            scriptableItem = weapon;

            UpdateUI();
        }

        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(e);
        }

        void UpdateUI()
        {
            itemImage.sprite = scriptableItem.icon;

            stars.SetRating(scriptableItem.itemTier);
        }
    }
}
