
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.Armoury.UI
{
    using GM.UI;

    public class ArmouryItemSlot : Core.GMMonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TMPro.TMP_Text nameText;
        [SerializeField] Image itemImage;
        [SerializeField] Button button;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        public void Init(int item)
        {
            GM.Armoury.Data.ArmouryItemData data = App.Data.Armoury.GetItem(item);

            itemImage.sprite    = data.Icon;
            nameText.text       = data.ItemName.ToUpper();

            stars.Show(data.Tier + 1);
        }


        public void SetButtonCallback(UnityAction e)
        {
            button.onClick.AddListener(e);
        }
    }
}
