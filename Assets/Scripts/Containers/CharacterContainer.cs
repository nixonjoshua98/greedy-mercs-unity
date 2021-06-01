using System;
using System.Linq;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

namespace GreedyMercs
{
    using GM.Characters;
    public class CharacterContainer
    {
        Dictionary<CharacterID, UpgradeState> characters;

        public CharacterContainer(JSONNode node)
        {
            characters = new Dictionary<CharacterID, UpgradeState>();

            foreach (JSONNode chara in node["characters"].AsArray)
            {
                characters[(CharacterID)int.Parse(chara["characterId"])] = JsonUtility.FromJson<UpgradeState>(chara.ToString());
            }
        }

        public void Clear()
        {
            characters = new Dictionary<CharacterID, UpgradeState>();
        }

        // === Helper Methods ===

        public UpgradeState Get(CharacterID chara) => characters[chara];

        public List<CharacterID> Unlocked() => characters.Keys.ToList();

        public bool Contains(CharacterID chara) => characters.ContainsKey(chara);

        public bool TryGetState(CharacterID chara, out UpgradeState result) => characters.TryGetValue(chara, out result);

        public void Add(CharacterID charaId)
        {
            characters[charaId] = new UpgradeState { level = 1 };
        }

        public Dictionary<BonusType, double> CalculateBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
            {
                if (GameState.Characters.TryGetState(hero, out var state))
                {
                    MercData data = StaticData.Mercs.GetMerc(hero);

                    foreach (MercPassiveData passive in data.Passives)
                    {
                        if (state.level >= passive.UnlockLevel)
                        {
                            if (passive.Value < 1)
                            {
                                bonuses[passive.Type] = bonuses.Get(passive.Type, 0) + passive.Value;
                            }

                            else
                                bonuses[passive.Type] = bonuses.Get(passive.Type, 1) * passive.Value;
                        }
                    }
                }
            }

            return bonuses;
        }

        public BigDouble CalcTapDamageBonus()
        {
            BigDouble val = 0;

            foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
            {
                if (GameState.Characters.TryGetState(hero, out var state))
                {
                    MercData data = StaticData.Mercs.GetMerc(hero);

                    foreach (MercPassiveData passive in data.Passives)
                    {
                        if (state.level >= passive.UnlockLevel)
                        {
                            if (passive.Type == BonusType.CHAR_TAP_DAMAGE_ADD)
                            {
                                val += passive.Value * StatsCache.CharacterDamage(hero);
                            }
                        }
                    }
                }
            }

            return val;
        }
    }
}