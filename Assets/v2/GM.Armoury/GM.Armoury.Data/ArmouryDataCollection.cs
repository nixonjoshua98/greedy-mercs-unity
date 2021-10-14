using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryDataCollection : Core.GMClass
    {
        List<Models.ArmouryItemUserDataModel> UserItemsList;
        List<Models.ArmouryItemGameDataModel> GameItemsList;

        public ArmouryDataCollection(List<Models.ArmouryItemUserDataModel> userItems, Models.ArmouryGameDataModel gameData)
        {
            UserItemsList = userItems;

            UpdateGameData(gameData);
        }

        /// <summary>
        /// Get user data for an item
        /// </summary>
        public Models.ArmouryItemUserDataModel GetUserItem(int key) => UserItemsList.Where(ele => ele.Id == key).FirstOrDefault();
        public List<Models.ArmouryItemUserDataModel> UserOwnedItems => UserItemsList.Where(ele => ele.NumOwned > 0 || ele.Level > 0).OrderBy(ele => ele.Id).ToList();

        /// <summary>
        /// Update the cached user data for a single item
        /// </summary>
        public void UpdateUserItem(Models.ArmouryItemUserDataModel item) => UserItemsList.UpdateOrInsertElement(item, (ele) => ele.Id == item.Id);
        public Models.ArmouryItemGameDataModel GetGameItem(int key) => GameItemsList.Where(ele => ele.Id == key).FirstOrDefault();

        /// <summary>
        /// Update all cached game data
        /// </summary>
        void UpdateGameData(Models.ArmouryGameDataModel data)
        {
            GameItemsList = data.Items;

            var localItemsDict = LoadLocalData();

            foreach (var item in GameItemsList)
            {
                var localMerc = localItemsDict[item.Id];

                item.Name = localMerc.Name;
                item.Icon = localMerc.Icon;

                item.BaseStarLevelCost = data.BaseStarLevelCost;
                item.MaxStarLevel = data.MaxStarLevel;
            }
        }

        /// <summary>
        /// Load local scriptable objects and return them as a dictionary
        /// </summary>
        Dictionary<int, ScriptableObjects.LocalArmouryItemData> LoadLocalData() =>
            Resources.LoadAll<ScriptableObjects.LocalArmouryItemData>("Armoury/Items").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Get a combined class of user data and game data for a single item
        /// </summary>
        public ArmouryItemData GetItem(int key) => new ArmouryItemData(GetGameItem(key), GetUserItem(key));


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

        public double TotalMercDamageMultiplier()
        {
            double dmgBonus = DamageBonus();

            return dmgBonus > 0 ? dmgBonus : 1;
        }

        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            double dmgBonus = DamageBonus();

            dmgBonus = dmgBonus > 0 ? dmgBonus : 1;

            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>
            {
                new KeyValuePair<BonusType, double>(BonusType.MERC_DAMAGE, dmgBonus)
            };

            return ls;
        }

        // === Server Methods === //
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

        public void EvolveItem(int item, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.EvolveArmouryItemRequest { ItemId = item };

            App.HTTP.Armoury_EvolveItem(req, (resp) =>
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
