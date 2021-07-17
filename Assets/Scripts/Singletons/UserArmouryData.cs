﻿
using System;
using System.Linq;

using System.Collections.Generic;

using SimpleJSON;

namespace GM.Armoury
{
    using GM.Data;

    public class ArmouryItemState
    {
        public int ID;

        public int level;
        public int owned;
        public int evoLevel;

        public ArmouryItemState(int itemId)
        {
            ID = itemId;
        }
    }

    public class UserArmouryData : IBonusManager
    {
        Dictionary<int, ArmouryItemState> states;

        public UserArmouryData(JSONNode node)
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
                }

                call();
            }

            Server.Post("armoury/upgrade", CreateJson(itemId), Callback);
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

            Server.Post("armoury/evolve", CreateJson(itemId), Callback);
        }


        // = = = GET = = =

        public List<ArmouryItemState> OwnedItems() => states.Values.Where(ele => ele.owned > 0).OrderBy(ele => ele.ID).ToList();

        public ArmouryItemState GetItem(int itemId)
        {
            if (!states.ContainsKey(itemId))
            {
                UnityEngine.Debug.LogError(string.Format("ArmouryManager.GetItem({0}) - KeyError", itemId));

                states[itemId] = new ArmouryItemState(itemId);
            }

            return states[itemId];
        }


        // = = = SET = = = //

        public void SetArmouryItems(JSONNode node)
        {
            states = new Dictionary<int, ArmouryItemState>();

            foreach (JSONNode item in node.AsArray)
            {
                int itemId = item["itemId"].AsInt;

                ArmouryItemState state = new ArmouryItemState(itemId)
                {
                    level = item["level"].AsInt,
                    owned = item["owned"].AsInt,
                    evoLevel = item["evoLevel"].AsInt
                };

                states[itemId] = state;
            }
        }

        // = = = Calculations = = = //

        public double WeaponDamage(int itemId)
        {
            ArmouryItemState state = GetItem(itemId);

            return WeaponDamage(itemId, state.level, state.evoLevel);
        }

        public double WeaponDamage(int itemId, int level)
        {
            ArmouryItemState state = GetItem(itemId);

            return WeaponDamage(itemId, level, state.evoLevel);
        }

        public double WeaponDamage(int itemId, int level, int evoLevel)
        {
            Data.ArmouryItemData item = GameData.Get().Armoury.Get(itemId);

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
                ArmouryItemState state = GetItem(w.Key);

                if (state.level > 0)
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