using SimpleJSON;
using System;
using System.Collections.Generic;

namespace GM
{
    using GM.Data;

    public class CharacterContainer : Core.GMClass
    {
        Dictionary<MercID, UpgradeState> characters;

        public CharacterContainer(JSONNode node)
        {
            characters = new Dictionary<MercID, UpgradeState>();
        }

        public void Clear()
        {
            characters = new Dictionary<MercID, UpgradeState>();
        }
        
        // === Helper Methods ===

        public bool TryGetState(MercID chara, out UpgradeState result) => characters.TryGetValue(chara, out result);

        public BigDouble CalcTapDamageBonus()
        {
            BigDouble val = 0;

            foreach (MercID merc in Enum.GetValues(typeof(MercID)))
            {
                if (GameState.Characters.TryGetState(merc, out var state))
                {
                    GM.Mercs.Data.FullMercData data = App.Data.Mercs.GetMerc(merc);

                    foreach (GM.Mercs.Data.MercPassiveSkillData passive in data.GameValues.Passives)
                    {
                        if (state.level >= passive.UnlockLevel)
                        {
                            if (passive.Type == BonusType.CHAR_TAP_DAMAGE_ADD)
                            {
                                val += passive.Value * StatsCache.TotalMercDamage(merc);
                            }
                        }
                    }
                }
            }

            return val;
        }
    }
}