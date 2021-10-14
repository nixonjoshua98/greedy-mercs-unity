using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using GM.Armoury.ScriptableObjects;

namespace GM.Armoury.Data
{
    public class ArmouryDataCollection : Core.GMClass
    {
        List<Models.ArmouryItemUserDataModel> UserItemsList;
        List<Models.ArmouryItemGameDataModel> GameItemsList;

        public ArmouryDataCollection(List<Models.ArmouryItemUserDataModel> userItems, List<Armoury.Models.ArmouryItemGameDataModel> gameData)
        {
            UserItemsList = userItems;

            UpdateGameData(gameData);
        }

        /// <summary>
        /// Get user data for an item
        /// </summary>
        public Models.ArmouryItemUserDataModel GetUserItem(int key) => UserItemsList.Where(ele => ele.Id == key).FirstOrDefault();

        /// <summary>
        /// </summary>
        public List<Models.ArmouryItemUserDataModel> UserOwnedItems => UserItemsList.Where(ele => ele.NumOwned > 0 || ele.Level > 0).OrderBy(ele => ele.Id).ToList();

        /// <summary>
        /// Update the cached user data for a single item
        /// </summary>
        public void UpdateUserItem(Models.ArmouryItemUserDataModel item) => UserItemsList.UpdateOrInsertElement(item, (ele) => ele.Id == item.Id);

        /// <summary>
        /// </summary>
        public Models.ArmouryItemGameDataModel GetGameItem(int key) => GameItemsList.Where(ele => ele.Id == key).FirstOrDefault();

        /// <summary>
        /// Update all cached game data
        /// </summary>
        void UpdateGameData(List<Models.ArmouryItemGameDataModel> data)
        {
            GameItemsList = data;

            var localItemsDict = LoadLocalData();

            foreach (var item in GameItemsList)
            {
                var localMerc = localItemsDict[item.Id];

                item.Name = localMerc.Name;
                item.Icon = localMerc.Icon;
            }
        }

        /// <summary>
        /// Load local scriptable objects and return them as a dictionary
        /// </summary>
        Dictionary<int, LocalArmouryItemData> LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Get a combined class of user data and game data for a single item
        /// </summary>
        public ArmouryItemData GetItem(int key) => new ArmouryItemData(GetGameItem(key), GetUserItem(key));

        /// <summary>
        /// </summary>
        public double DamageBonus()
        {
            double val = 0;

            foreach (Models.ArmouryItemUserDataModel item in UserOwnedItems)
            {
                ArmouryItemData itemData = GetItem(item.Id);

                if (itemData.CurrentLevel > 0)
                {
                    val += itemData.WeaponDamage;
                }
            }

            return val;
        }

        /// <summary>
        /// </summary>
        public double TotalMercDamageMultiplier()
        {
            double dmgBonus = DamageBonus();

            return dmgBonus > 0 ? dmgBonus : 1;
        }

        /// <summary>
        /// </summary>
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            double dmgBonus = TotalMercDamageMultiplier();

            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>
            {
                new KeyValuePair<BonusType, double>(BonusType.MERC_DAMAGE, dmgBonus)
            };

            return ls;
        }

        /// <summary>
        /// </summary>
        public void UpgradeItem(int item, UnityAction<bool> call)
        {
            var req = new GM.HTTP.Requests.UpgradeArmouryItemRequest { ItemId = item };

            App.HTTP.Armoury_UpgradeItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    UpdateUserItem(resp.UpdatedItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call(resp.StatusCode == 200);
            });
        }

        /// <summary>
        /// </summary>
        public void EvolveItem(int item, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.UpgradeStarLevelArmouryItemRequest { ItemId = item };

            App.HTTP.Armoury_UpgradeStarLevelItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    UpdateUserItem(resp.UpdatedItem);
                }

                call(resp.StatusCode == 200);
            });
        }
    }
}
