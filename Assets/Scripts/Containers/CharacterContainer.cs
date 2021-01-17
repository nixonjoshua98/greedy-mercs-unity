using System;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

using PassiveSkill      = PassivesData.PassiveSkill;
using CharacterID       = CharacterData.CharacterID;
using HeroPassiveUnlock = CharacterData.HeroPassiveUnlock;

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
                List<HeroPassiveUnlock> heroPassiveUnlocks = StaticData.Characters.GetPassives(hero);

                foreach (HeroPassiveUnlock unlock in heroPassiveUnlocks)
                {
                    if (state.level >= unlock.unlockLevel)
                    {
                        PassiveSkill skill = StaticData.Passives.Get(unlock.skill);

                        if (skill.value < 1)
                        {
                            bonuses[skill.bonusType] = bonuses.GetOrVal(skill.bonusType, 0) + skill.value;
                        }

                        else
                            bonuses[skill.bonusType] = bonuses.GetOrVal(skill.bonusType, 1) * skill.value;
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
                foreach (HeroPassiveUnlock unlock in StaticData.Characters.GetPassives(hero))
                {
                    if (state.level >= unlock.unlockLevel)
                    {
                        PassiveSkill skill = StaticData.Passives.Get(unlock.skill);

                        if (skill.bonusType == BonusType.CHAR_TAP_DAMAGE_ADD)
                        {
                            val += skill.value * StatsCache.GetCharacterDamage(hero);
                        }
                    }
                }
            }
        }
        
        return val;
    }
}
