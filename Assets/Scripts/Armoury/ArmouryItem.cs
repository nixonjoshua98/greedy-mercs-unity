
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.Armoury.UI
{
    using GM.Armoury;

    using GM.UI;

    public class ArmouryItem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image itemImage;
        [Space]
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        public void Init(ArmouryItemData itemData)
        {
            itemImage.sprite = itemData.Icon;

            stars.Show(itemData.Tier);
        }

        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(e);
        }
    }
}
