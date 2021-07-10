using System;
using System.Linq;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

namespace GM
{
    using GM.Units;
    public class CharacterContainer
    {
        Dictionary<UnitID, UpgradeState> characters;

        public CharacterContainer(JSONNode node)
        {
            characters = new Dictionary<UnitID, UpgradeState>();
        }

        public void Clear()
        {
            characters = new Dictionary<UnitID, UpgradeState>();
        }
        
        // === Helper Methods ===

        public bool TryGetState(UnitID chara, out UpgradeState result) => characters.TryGetValue(chara, out result);

        public BigDouble CalcTapDamageBonus()
        {
            BigDouble val = 0;

            foreach (UnitID hero in Enum.GetValues(typeof(UnitID)))
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
                                val += passive.Value * StatsCache.TotalMercDamage(hero);
                            }
                        }
                    }
                }
            }

            return val;
        }
    }
}