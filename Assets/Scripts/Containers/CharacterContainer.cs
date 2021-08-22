using SimpleJSON;
using System;
using System.Collections.Generic;

namespace GM
{
    using GM.Data;

    public class CharacterContainer
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

                    MercData data = GameData.Get.Mercs.Get(merc);

                    foreach (MercPassiveData passive in data.Passives)
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