using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GM.Armoury.Data
{
    public class ArmouryData : Core.GMClass
    {
        public GameArmouryDictionary Game;
        public UserArmouryDictionary User;

        public ArmouryData(JSONNode userJSON, JSONNode gameJSON)
        {
            Game = new GameArmouryDictionary(gameJSON);
            User = new UserArmouryDictionary(userJSON);
        }


        // Index accessors
        public FullArmouryItemData this[int key]
        {
            get => new FullArmouryItemData(Game[key], User[key]);
        }


        public double DamageBonus()
        {
            double val = 0;

            foreach (KeyValuePair<int, ArmouryItemState> pair in User)
            {
                FullArmouryItemData item = this[pair.Key];

                if (item.User.Level > 0)
                {
                    val += item.WeaponDamage;
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
            App.HTTP.Post("armoury/upgrade", CreateJSON(item), (code, resp) => {

                if (code == 200)
                {
                    User.UpdateWithJSON(resp["userArmouryItems"]);

                    App.Data.Inv.UpdateCurrenciesWithJSON(resp["userItems"]);
                }

                call(code == 200);
            });
        }

        /// <summary>
        /// Send the request to evolve an item
        /// </summary>
        /// <param name="item">Armoury item ID</param>
        /// <param name="call">Callback</param>
        public void EvolveItem(int item, UnityAction<bool> call)
        {
            App.HTTP.Post("armoury/evolve", CreateJSON(item), (code, resp) => {

                if (code == 200)
                {
                    User.UpdateWithJSON(resp["userArmouryItems"]);
                }

                call(code == 200);
            });
        }

        static JSONNode CreateJSON(int item)
        {
            JSONNode node = new JSONObject();

            node["itemId"] = item;

            return node;
        }
    }
}
