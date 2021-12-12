using GM.Armoury.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using GM.Armoury.Models;

namespace GM.Armoury.Data
{
    public class Armoury : Core.GMClass
    {

        Dictionary<int, ArmouryItemUserDataModel> UserItemsDict;
        Dictionary<int, ArmouryItemGameDataModel> GameItemsDict;

        public Armoury(List<ArmouryItemUserDataModel> userItems, List<ArmouryItemGameDataModel> gameData)
        {
            Update(userItems);
            Update(gameData);
        }

        public List<ArmouryItemUserDataModel> UserItems => UserItemsDict.Values.Where(ele => DoesUserOwnItem(ele.Id)).OrderBy(ele => ele.Id).ToList();
        public List<ArmouryItemGameDataModel> GameItems => GameItemsDict.Values.OrderBy(ele => ele.Id).ToList();

        /// <summary>Update the cached user data for a single item</summary>
        public void Update(ArmouryItemUserDataModel item) => UserItemsDict[item.Id] = item;

        /// <summary>Update all cached game data</summary>
        void Update(List<ArmouryItemGameDataModel> data)
        {
            var localItemsDict = LoadLocalData();

            data.ForEach(item =>
            {
                var local = localItemsDict[item.Id];

                item.Name = local.Name;
                item.Icon = local.Icon;
            });

            GameItemsDict = data.ToDictionary(ele => ele.Id, ele => ele);
        }

        /// <summary>Update user items data</summary>
        void Update(List<ArmouryItemUserDataModel> userItems) => UserItemsDict = userItems.ToDictionary(ele => ele.Id, ele => ele);

        public ArmouryItemGameDataModel GetGameItem(int key) => GameItemsDict[key];

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

        /// <summary>Check if the user owns the item</summary>
        public bool DoesUserOwnItem(int key)
        {
            if (UserItemsDict.TryGetValue(key, out var result))
            {
                return result.NumOwned > 0 || result.Level > 0;
            }

            return false;
        }

        /// <summary>Load local scriptable objects and return them as a dictionary</summary>
        Dictionary<int, LocalArmouryItemData> LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>Get a combined class of user data and game data for a single item</summary>
        public ArmouryItemData GetItem(int key) => new ArmouryItemData(GetGameItem(key), UserItemsDict[key]);

        /// <summary>
        /// </summary>
        public double DamageBonus()
        {
            double val = 0;

            foreach (ArmouryItemUserDataModel item in UserItems)
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

        public void MergeItem(int item, UnityAction<bool> call)
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
