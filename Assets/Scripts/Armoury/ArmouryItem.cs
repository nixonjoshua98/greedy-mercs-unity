
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.Armoury.UI
{
    using GM.Data;

    using GM.UI;

    public class ArmouryItem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image itemImage;
        [Space]
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        public void Init(int itemId)
        {
            ArmouryItemData data = GameData.Get().Armoury.Get(itemId);

            itemImage.sprite = data.Icon;

            stars.Show(data.Tier);
        }

        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(e);
        }
    }
}
