using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{
    using Formulas = GM.Formulas;

    public class MercState
    {
        public MercID Id;

        public int Level;
        public GM.Mercs.Data.MercPassiveSkillData[] UnlockedPassives { get { return _svrData.Passives.Where(passive => Level >= passive.UnlockLevel).ToArray(); } }

        GM.Mercs.Data.MercGameData _svrData => Core.GMApplication.Instance.Data.Mercs.GetMerc(Id).GameValues;

        public BigDouble CostToUpgrade(int levels)
        {
            return Formulas.MercLevelUpCost(Level, levels, _svrData.UnlockCost);
        }

    }

    public class MercenaryManager
    {
        public static MercenaryManager Instance = null;

        Dictionary<MercID, MercState> states;

        public static void Create()
        {
            Instance = new MercenaryManager();
        }

        MercenaryManager()
        {
            states = new Dictionary<MercID, MercState>();
        }

        // = = = Get Methods = = = //
        public List<MercID> Unlocked { get { return states.Keys.ToList(); } }

        public MercState GetState(MercID chara)
        {
            if (!Contains(chara))
                SetState(chara);

            return states[chara];
        }

        public bool Contains(MercID chara)
        {
            return states.ContainsKey(chara);
        }

        public bool GetNextHero(out MercID result)
        {
            result = (MercID)(-1);

            foreach (MercID chara in Enum.GetValues(typeof(MercID)))
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
        public void SetState(MercID chara)
        {
            states[chara] = new MercState() { Id = chara, Level = 1 };
        }

        public void SetState(MercID chara, MercState state)
        {
            states[chara] = state;
        }

        public void AddLevels(MercID chara, int levels)
        {
            MercState state = GetState(chara);

            state.Level += levels;

            SetState(chara, state);
        }

        // = = = Special Methods = = = //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (MercID chara in Unlocked)
            {
                MercState state = GetState(chara);

                foreach (GM.Mercs.Data.MercPassiveSkillData passive in state.UnlockedPassives)
                {
                    ls.Add(new KeyValuePair<BonusType, double>(passive.Type, passive.Value));
                }
            }
            
            return ls;
        }
    }
}