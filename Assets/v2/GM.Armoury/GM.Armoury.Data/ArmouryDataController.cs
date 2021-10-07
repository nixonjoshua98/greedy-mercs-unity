using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GM.Armoury.Data
{
    public class ArmouryDataController : Core.GMClass
    {
        public ArmouryGameDataCollection Game;
        public ArmouryUserDataCollection User;

        public ArmouryDataController(JSONNode userJSON, JSONNode gameJSON)
        {
            Game = new ArmouryGameDataCollection(gameJSON);
            User = new ArmouryUserDataCollection(userJSON);
        }

        public FullArmouryItemData GetItem(int key) => new FullArmouryItemData(Game[key], User.GetItem(key));


        public double DamageBonus()
        {
            double val = 0;

            foreach (Models.UserArmouryItemModel item in User.OwnedItems)
            {
                FullArmouryItemData itemData = GetItem(item.Id);

                if (itemData.User.Level > 0)
                {
                    val += itemData.WeaponDamage;
                }
            }

            return val;
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

        /// <summary>
        /// Send the request to upgrade an item
        /// </summary>
        /// <param name="item">Armoury item ID</param>
        /// <param name="call">Callback</param>
        public void UpgradeItem(int item, UnityAction<bool> call)
        {
            var req = new GM.HTTP.Requests.UpgradeArmouryItemRequest { ItemId = item };

            App.HTTP.Armoury_UpgradeItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.Update(resp.UpdatedItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call(resp.StatusCode == 200);
            });
        }

        /// <summary>
        /// Send the request to evolve an item
        /// </summary>
        /// <param name="item">Armoury item ID</param>
        /// <param name="call">Callback</param>
        public void EvolveItem(int item, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.EvolveArmouryItemRequest { ItemId = item };

            App.HTTP.Armoury_EvolveItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.Update(resp.UpdatedItem);
                }

                call(resp.StatusCode == 200);
            });
        }
    }
}
