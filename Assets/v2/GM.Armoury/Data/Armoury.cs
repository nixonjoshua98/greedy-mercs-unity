using GM.Armoury.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Armoury.Data
{
    public class Armoury : Core.GMClass
    {

        Dictionary<int, Models.ArmouryItemUserDataModel> UserItemsDict;
        Dictionary<int, Models.ArmouryItemGameDataModel> GameItemsDict;

        public Armoury(List<Models.ArmouryItemUserDataModel> userItems, List<Models.ArmouryItemGameDataModel> gameData)
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

        public void UpgradeItem(int itemId, UnityAction<bool> call)
        {
            App.HTTP.UpgradeArmouryItem(itemId, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    Update(resp.Item);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }

        public void EvolveItem(int item, UnityAction<bool> call)
        {
            App.HTTP.MergeArmouryItem(item, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    Update(resp.Item);
                }

                call(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }
    }
}
