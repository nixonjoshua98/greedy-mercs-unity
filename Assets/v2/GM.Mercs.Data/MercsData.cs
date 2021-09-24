using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

namespace GM.Mercs.Data
{
    public class MercsData
    {
        public MercGameDataDictionary Game;
        public MercUserDataDictionary User;

        public MercsData(JSONNode gameJSON)
        {
            Game = new MercGameDataDictionary(gameJSON);
            User = new MercUserDataDictionary();
        }


        public FullMercData this[MercID key]
        {
            get => new FullMercData(Game[key], User[key]);
        }


        public bool IsMercUnlocked(MercID chara)
        {
            return User.ContainsKey(chara);
        }


        public bool GetNextHero(out MercID result)
        {
            result = (MercID)(-1);

            foreach (MercID chara in System.Enum.GetValues(typeof(MercID)))
            {
                if ((int)chara >= 0 && !IsMercUnlocked(chara))
                {
                    result = chara;

                    return true;
                }
            }

            return false;
        }


        public FullMercData[] UnlockedMercs => User.Unlocked.Select(merc => this[merc]).ToArray();


        // = = = Special Methods = = = //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (FullMercData merc in UnlockedMercs)
            {
                foreach (MercPassiveSkillData passive in merc.UnlockedPassives)
                {
                    ls.Add(new KeyValuePair<BonusType, double>(passive.Type, passive.Value));
                }
            }

            return ls;
        }


        public BigDouble CalcTapDamageBonus()
        {
            BigDouble val = 0;

            foreach (MercID merc in System.Enum.GetValues(typeof(MercID)))
            {
                if (User.TryGetValue(merc, out MercUserData state))
                {
                    foreach (MercPassiveSkillData passive in this[merc].UnlockedPassives)
                    {
                        if (passive.Type == BonusType.CHAR_TAP_DAMAGE_ADD)
                        {
                            val += passive.Value * StatsCache.TotalMercDamage(merc);
                        }
                    }
                }
            }

            return val;
        }

    }
}
