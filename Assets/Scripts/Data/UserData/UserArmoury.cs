
using System;
using System.Linq;

using System.Collections.Generic;

using SimpleJSON;

namespace GM.Data
{
    using GM.HTTP;

    public class UserArmoury
    {
        Dictionary<int, GM.Armoury.Data.ArmouryItemState> states;

        public UserArmoury(JSONNode node)
        {
            SetArmouryItems(node);
        }


        // = = = Server Methods = = =
        public void UpgradeItem(int itemId, Action call)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    SetArmouryItems(resp["userArmouryItems"]);
                    UserData.Get.Inventory.SetServerItemData(resp["userItems"]);
                }

                call();
            }

            HTTPClient.GetClient().Post("armoury/upgrade", CreateJson(itemId), Callback);
        }


        public void EvolveItem(int itemId, Action call)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    SetArmouryItems(resp["userArmouryItems"]);
                }

                call();
            }

            HTTPClient.GetClient().Post("armoury/evolve", CreateJson(itemId), Callback);
        }


        // = = = GET = = =

        public List<GM.Armoury.Data.ArmouryItemState> OwnedItems() => states.Values.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.ID).ToList();

        public GM.Armoury.Data.ArmouryItemState Get(int itemId)
        {
            if (!states.ContainsKey(itemId))
            {
                UnityEngine.Debug.LogError(string.Format("ArmouryManager.GetItem({0}) - KeyError", itemId));

                states[itemId] = new GM.Armoury.Data.ArmouryItemState(itemId);
            }

            return states[itemId];
        }


        // = = = SET = = = //

        public void SetArmouryItems(JSONNode node)
        {
            states = new Dictionary<int, GM.Armoury.Data.ArmouryItemState>();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                int itemId = int.Parse(key);

                states[itemId] = new GM.Armoury.Data.ArmouryItemState(itemId)
                {
                    Level = current["level"].AsInt,
                    NumOwned = current["owned"].AsInt,
                    EvoLevel = current["evoLevel"].AsInt
                };
            }
        }

        // = = = Calculations = = = //

        public double WeaponDamage(int itemId)
        {
            GM.Armoury.Data.ArmouryItemState state = Get(itemId);

            return WeaponDamage(itemId, state.Level, state.EvoLevel);
        }

        public double WeaponDamage(int itemId, int level, int evoLevel)
        {
            GM.Armoury.Data.ArmouryItemData item = GameData.Get.Armoury.Get(itemId);

            double val = ((evoLevel + 1) * ((item.BaseDamageMultiplier) - 1) * level) + 1;

            return val > 1 ? val : 0;
        }


        // = = = Internal = = = //

        JSONNode CreateJson(int itemId)
        {
            JSONNode node = new JSONObject();

            node["itemId"] = itemId;

            return node;
        }

        // = = = Special Methods = = = //

        public double DamageBonus()
        {
            double val = 0;

            foreach (var w in states)
            {
                GM.Armoury.Data.ArmouryItemState state = Get(w.Key);

                if (state.Level > 0)
                    val += WeaponDamage(w.Key);
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
    }
}
