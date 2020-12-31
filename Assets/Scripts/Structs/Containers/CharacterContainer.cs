using System;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

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

    public UpgradeState GetCharacter(CharacterID chara) 
    { 
        return characters[chara]; 
    }

    public bool TryGetHeroState(CharacterID chara, out UpgradeState result) 
    {
        return characters.TryGetValue(chara, out result);
    }

    public void AddHero(CharacterID charaId)
    {
        characters[charaId] = new UpgradeState { level = 1 };
    }

    public Dictionary<BonusType, double> CalculateBonuses()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

        foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
        {
            if (GameState.Characters.TryGetHeroState(hero, out var state))
            {
                List<HeroPassiveUnlock> heroPassiveUnlocks = StaticData.Characters.GetPassives(hero);

                foreach (HeroPassiveUnlock unlock in heroPassiveUnlocks)
                {
                    if (state.level >= unlock.unlockLevel)
                    {
                        PassiveSkill skill = StaticData.Passives.Get(unlock.skill);

                        switch (skill.bonusType)
                        {
                            case BonusType.HERO_TAP_DAMAGE_ADD:
                                bonuses[skill.bonusType] = bonuses.GetValueOrDefault(skill.bonusType, 0) + skill.value;
                                break;

                            default:
                                bonuses[skill.bonusType] = bonuses.GetValueOrDefault(skill.bonusType, 1) * skill.value;
                                break;
                        }
                    }
                }
            }
        }

        return bonuses;
    }
}
