using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    using GM.Armoury;

    using GM.UI;
    using GM.Data;

    public class ArmouryItemPopup : MonoBehaviour
    {
        [Header("Upgrade Components")]
        [SerializeField] Text upgradeDamageText;

        [Header("Evolve Components")]
        [SerializeField] Text evoDamageText;
        [SerializeField] Slider evolveSlider;

        [Header("Item Components")]
        [SerializeField] Image colouredWeapon;
        [SerializeField] Image shadowWeapon;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        int _itemId;


        public void Init(int itemId)
        {
            _itemId = itemId;

            Data.ArmouryItemData data = GetData();

            colouredWeapon.sprite = shadowWeapon.sprite = data.Icon;

            stars.Show(data.Tier);

            UpdateUI();
        }


        void UpdateUI()
        {
            // We use the manager a lot, so we cache it temporary
            UserArmouryData armoury = UserData.Get().Armoury;

            // Grab the current state
            ArmouryItemState state  = armoury.GetItem(_itemId);

            // Calculate the values we need
            double currentDamage    = armoury.WeaponDamage(_itemId);
            double nextLevelDamage  = armoury.WeaponDamage(_itemId, state.level + 1);
            double nextEvoDamage    = armoury.WeaponDamage(_itemId, state.level, state.evoLevel + 1);

            // Format the above values as strings
            string currentDmgString = FormatString.Number(currentDamage * 100, prefix: "%");
            string nextEvoDmgString = FormatString.Number(nextEvoDamage * 100, prefix: "%");
            string nextLvlDmgString = FormatString.Number(nextLevelDamage * 100, prefix: "%");

            // Set the text widgets
            upgradeDamageText.text  = string.Format("{0} -> {1}", currentDmgString, nextLvlDmgString);
            evoDamageText.text      = string.Format("{0} -> {1}", currentDmgString, nextEvoDmgString);

            // Update the evolve level slider
            evolveSlider.maxValue   = 5;
            evolveSlider.value      = state.owned;
        }


        Data.ArmouryItemData GetData() => GameData.Get().Armoury.Get(_itemId);


        // = = = Button Callbacks = = = //

        public void OnEvolveButton()
        {
            UserData.Get().Armoury.EvolveItem(_itemId, () => { UpdateUI(); });
        }


        public void OnUpgradeButton()
        {
            UserData.Get().Armoury.UpgradeItem(_itemId, () => { UpdateUI(); });
        }


        public void OnClosePopup() 
        { 
            Destroy(gameObject); 
        }
    }
}
