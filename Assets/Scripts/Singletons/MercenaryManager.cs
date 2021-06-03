using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Characters
{
    using StatsCache = GreedyMercs.StatsCache;
    using Formulas = GreedyMercs.Formulas;

    public class MercState
    {
        public CharacterID ID;

        public int Level;
        public MercPassiveData[] UnlockedPassives { get { return _svrData.Passives.Where(passive => Level >= passive.UnlockLevel).ToArray(); } }

        MercData _svrData { get { return StaticData.Mercs.GetMerc(ID); } }  // Quick, dirty reference


        public BigDouble CostToUpgrade(int levels)
        {
            return Formulas.MercLevelUpCost(Level, levels, _svrData.UnlockCost);
        }

        public BigDouble TotalDamage()
        {
            return StatsCache.TotalMercDamage(ID);
        }

    }

    public class MercenaryManager : IBonusManager
    {
        public static MercenaryManager Instance = null;

        Dictionary<CharacterID, MercState> states;

        public static void Create()
        {
            Instance = new MercenaryManager();
        }

        MercenaryManager()
        {
            states = new Dictionary<CharacterID, MercState>();
        }

        // = = = Get Methods = = = //
        public List<CharacterID> Unlocked { get { return states.Keys.ToList(); } }

        public MercState GetState(CharacterID chara)
        {
            if (!Contains(chara))
                SetState(chara);

            return states[chara];
        }

        public bool Contains(CharacterID chara)
        {
            return states.ContainsKey(chara);
        }

        public bool GetNextHero(out CharacterID result)
        {
            result = (CharacterID)(-1);

            foreach (CharacterID chara in Enum.GetValues(typeof(CharacterID)))
            {
                if ((int)chara >= 0 && !Contains(chara))
                {
                    result = chara;

                    return true;
                }
            }

            return false;
        }

        // = = = Set Methods = = = //
        public void SetState(CharacterID chara)
        {
            states[chara] = new MercState() { ID = chara, Level = 1 };
        }

        public void SetState(CharacterID chara, MercState state)
        {
            states[chara] = state;
        }

        public void AddLevels(CharacterID chara, int levels)
        {
            MercState state = GetState(chara);

            state.Level += levels;

            SetState(chara, state);
        }

        // = = = Special Methods = = = //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (CharacterID chara in Unlocked)
            {
                MercState state = GetState(chara);

                foreach (MercPassiveData passive in state.UnlockedPassives)
                {
                    ls.Add(new KeyValuePair<BonusType, double>(passive.Type, passive.Value));
                }
            }
            
            return ls;
        }
    }
}