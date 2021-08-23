
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.Armoury.UI
{
    using GM.Data;

    using GM.UI;

    public class ArmouryItemSlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text nameText;
        [SerializeField] Image itemImage;
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        public void Init(int itemId)
        {
            ArmouryItemData data = GameData.Get.Armoury.Get(itemId);

            itemImage.sprite    = data.Icon;
            nameText.text       = data.Name.ToUpper();

            stars.Show(data.Tier + 1);
        }


        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.AddListener(e);
        }
    }
}
