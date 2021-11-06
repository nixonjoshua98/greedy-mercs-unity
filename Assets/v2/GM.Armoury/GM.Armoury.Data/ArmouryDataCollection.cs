using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using GM.Armoury.ScriptableObjects;
using System;

namespace GM.Armoury.Data
{
    public class ArmouryDataCollection : Core.GMClass
    {

        Dictionary<int, Models.ArmouryItemUserDataModel> UserItemsDict;
        Dictionary<int, Models.ArmouryItemGameDataModel> GameItemsDict;

        public ArmouryDataCollection(List<Models.ArmouryItemUserDataModel> userItems, List<Models.ArmouryItemGameDataModel> gameData)
        {
            Update(userItems);
            Update(gameData);
        }

        /// <summary>
        /// </summary>
        public List<Models.ArmouryItemUserDataModel> UserOwnedItems => UserItemsDict.Values.Where(ele => DoesUserOwnItem(ele.Id)).OrderBy(ele => ele.Id).ToList();

        public int NumUnlockedItems => UserOwnedItems.Count;

        public int NumItems => GameItemsDict.Count;

        /// <summary>
        /// Update the cached user data for a single item
        /// </summary>
        public void Update(Models.ArmouryItemUserDataModel item) => UserItemsDict[item.Id] = item;

        /// <summary>
        /// Update all cached game data
        /// </summary>
        void Update(List<Models.ArmouryItemGameDataModel> data)
        {
            GameItemsDict = data.ToDictionary(ele => ele.Id, ele => ele);

            var localItemsDict = LoadLocalData();

            foreach (var item in GameItemsDict.Values)
            {
                var localMerc = localItemsDict[item.Id];

                item.Name = localMerc.Name;
                item.Icon = localMerc.Icon;
            }
        }

        /// <summary>
        /// Update user items data
        /// </summary>
        void Update(List<Models.ArmouryItemUserDataModel> userItems) => UserItemsDict = userItems.ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// </summary>
        public Models.ArmouryItemGameDataModel GetGameItem(int key) => GameItemsDict[key];

        public bool TryGetOwnedItem(int key, out ArmouryItemData result)
        {
            result = default;

            if (DoesUserOwnItem(key))
            {
                result = GetItem(key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the user owns the item
        /// </summary>
        public bool DoesUserOwnItem(int key)
        {
            if (UserItemsDict.TryGetValue(key, out var result))
            {
                return result.NumOwned > 0 || result.Level > 0;
            }

            return false;
        }

        /// <summary>
        /// Load local scriptable objects and return them as a dictionary
        /// </summary>
        Dictionary<int, LocalArmouryItemData> LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Get a combined class of user data and game data for a single item
        /// </summary>
        public ArmouryItemData GetItem(int key) => new ArmouryItemData(GetGameItem(key), UserItemsDict[key]);

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
        public double ArmouryMercDamageMultiplier => Math.Max(1, DamageBonus());

        /// <summary>
        /// </summary>
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            double dmgBonus = ArmouryMercDamageMultiplier;

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

            App.HTTP.Armoury_Upgrade(req, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    Update(resp.UpdatedItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }

        /// <summary>
        /// </summary>
        public void EvolveItem(int item, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.UpgradeStarLevelArmouryItemRequest { ItemId = item };

            App.HTTP.Armoury_Merge(req, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    Update(resp.UpdatedItem);
                }

                call(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }
    }
}
