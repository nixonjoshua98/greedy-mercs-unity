using GM.Armoury.Models;
using GM.Armoury.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UpgradeArmouryItemResponse = GM.HTTP.Requests.UpgradeArmouryItemResponse;

namespace GM.Armoury.Data
{
    public class ArmouryDataContainer : Core.GMClass
    {
        private Dictionary<int, ArmouryItemUserData> UserModels;
        private Dictionary<int, ArmouryItem> GameItemsDict;

        public void Set(List<ArmouryItemUserData> userItems, List<ArmouryItem> gameData)
        {
            UserModels = userItems.ToDictionary(ele => ele.Id, ele => ele);

            SetStaticData(gameData);
        }

        public List<AggregatedArmouryItem> UserItems => UserModels.Values.Where(ele => DoesUserOwnItem(ele.Id)).OrderBy(ele => ele.Id).Select(x => GetItem(x.Id)).ToList();

        /// <summary>
        /// Update the cached user data for a single item
        /// </summary>
        public void Update(ArmouryItemUserData item)
        {
            UserModels[item.Id] = item;
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

        public ArmouryItem GetGameItem(int key) => GameItemsDict[key];

        public bool TryGetOwnedItem(int itemId, out ArmouryItemUserData result) => UserModels.TryGetValue(itemId, out result);

        /// <summary>
        /// Check if the user owns the item
        /// </summary>
        public bool DoesUserOwnItem(int itemId) => UserModels.TryGetValue(itemId, out var result) && result.NumOwned > 0;

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
            if (!GameItemsDict.ContainsKey(itemId))
                throw new System.Exception($"Armoury item '{itemId}' not found");

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
