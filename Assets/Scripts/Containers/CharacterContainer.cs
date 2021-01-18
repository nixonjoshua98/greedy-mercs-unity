using System;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

namespace Data.Characters
{
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

        public JSONNode ToJson()
        {
            return Utils.Json.CreateJSONArray("characterId", characters);
        }

        // === Helper Methods ===

        public UpgradeState Get(CharacterID chara)
        {
            return characters[chara];
        }

        public bool Contains(CharacterID chara) => characters.ContainsKey(chara);

        public bool TryGetState(CharacterID chara, out UpgradeState result) => characters.TryGetValue(chara, out result);

        public void Add(CharacterID charaId)
        {
            characters[charaId] = new UpgradeState { level = 1 };
        }

        public Dictionary<BonusType, double> CalcBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
            {
                if (GameState.Characters.TryGetState(hero, out var state))
                {
                    List<CharacterPassive> passives = StaticData.CharacterList.Get(hero).passives;

                    foreach (CharacterPassive passive in passives)
                    {
                        if (state.level >= passive.unlockLevel)
                        {
                            if (passive.value < 1)
                            {
                                bonuses[passive.bonusType] = bonuses.GetOrVal(passive.bonusType, 0) + passive.value;
                            }

                            else
                                bonuses[passive.bonusType] = bonuses.GetOrVal(passive.bonusType, 1) * passive.value;
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
                    List<CharacterPassive> passives = StaticData.CharacterList.Get(hero).passives;

                    foreach (CharacterPassive passive in passives)
                    {
                        if (state.level >= passive.unlockLevel)
                        {
                            if (passive.bonusType == BonusType.CHAR_TAP_DAMAGE_ADD)
                            {
                                val += passive.value * StatsCache.GetCharacterDamage(hero);
                            }
                        }
                    }
                }
            }

            return val;
        }
    }
}