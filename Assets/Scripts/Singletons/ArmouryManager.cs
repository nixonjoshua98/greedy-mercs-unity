
using System;

using System.Collections.Generic;

using SimpleJSON;

namespace GM.Armoury
{
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

    public class ArmouryManager : IBonusManager
    {
        public static ArmouryManager Instance = null;

        Dictionary<int, ArmouryItemState> states;

        public static ArmouryManager Create(JSONNode node)
        {
            Instance = new ArmouryManager();

            Instance.SetArmouryItems(node);

            return Instance;
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
        public List<ArmouryItemState> GetOwned()
        {
            List<ArmouryItemState> ls = new List<ArmouryItemState>();

            foreach (ArmouryItemState state in states.Values)
            {
                if (state.owned > 0)
                    ls.Add(state);
            }

            return ls;
        }

        public ArmouryItemState GetItem(int index)
        {
            if (!states.ContainsKey(index))
                states[index] = new ArmouryItemState(index);

            return states[index];
        }

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

        // = = = Internal = = =
        JSONNode CreateJson(int itemId)
        {
            JSONNode node = Utils.Json.GetDeviceInfo();

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
                    val += Formulas.Armoury.WeaponDamage(w.Key);
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
