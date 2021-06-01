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
        }

        public void Clear()
        {
            characters = new Dictionary<CharacterID, UpgradeState>();
        }
        
        // === Helper Methods ===

        public bool TryGetState(CharacterID chara, out UpgradeState result) => characters.TryGetValue(chara, out result);

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