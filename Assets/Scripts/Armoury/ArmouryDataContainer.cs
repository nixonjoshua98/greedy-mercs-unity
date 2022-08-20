using SRC.Armoury.Requests;
using SRC.Armoury.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.Armoury.Data
{
    public class ArmouryDataContainer : Core.GMClass
    {
        private Dictionary<int, UserArmouryItem> UserModels;
        private Dictionary<int, ArmouryItem> GameItemsDict;

        public void Set(List<UserArmouryItem> userItems, List<ArmouryItem> gameData)
        {
            UserModels = userItems.ToDictionary(ele => ele.ID, ele => ele);

            SetStaticData(gameData);
        }

        public List<AggregatedArmouryItem> UserItems => UserModels.Values.Where(ele => DoesUserOwnItem(ele.ID)).OrderBy(ele => ele.ID).Select(x => GetItem(x.ID)).ToList();

        /// <summary>
        /// Update the cached user data for a single item
        /// </summary>
        public void Update(UserArmouryItem item)
        {
            UserModels[item.ID] = item;
        }

        /// <summary>
        /// Update all cached game data
        /// </summary>
        private void SetStaticData(List<ArmouryItem> data)
        {
            var localItemsDict = LoadLocalData();

            data.ForEach(item =>
            {
                item.Icon = localItemsDict[item.ID].Icon;
            });

            GameItemsDict = data.ToDictionary(ele => ele.ID, ele => ele);
        }

        public ArmouryItem GetGameItem(int key)
        {
            return GameItemsDict[key];
        }

        public bool TryGetOwnedItem(int itemId, out UserArmouryItem result)
        {
            return UserModels.TryGetValue(itemId, out result);
        }

        /// <summary>
        /// Check if the user owns the item
        /// </summary>
        public bool DoesUserOwnItem(int itemId)
        {
            return UserModels.TryGetValue(itemId, out var result) && result.NumOwned > 0;
        }

        /// <summary>
        /// Load local scriptable objects and return them as a dictionary
        /// </summary>
        private Dictionary<int, ArmouryItemScriptableObject> LoadLocalData()
        {
            return Resources.LoadAll<ArmouryItemScriptableObject>("Scriptables/Armoury").ToDictionary(ele => ele.Id, ele => ele);
        }

        /// <summary>
        /// Get a combined class of user data and game data for a single item
        /// </summary>
        public AggregatedArmouryItem GetItem(int itemId)
        {
            return new(itemId);
        }

        public void UpgradeItem(int itemId, UnityAction<bool, UpgradeArmouryItemResponse> call)
        {
            App.HTTP.UpgradeArmouryItem(itemId, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    Update(resp.Item);

                    App.Inventory.ArmouryPoints -= resp.UpgradeCost;
                }

                call(resp.StatusCode == HTTP.HTTPCodes.Success, resp);
            });
        }
    }
}
