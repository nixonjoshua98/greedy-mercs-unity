﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Characters
{
    public class MercState
    {
        public int Level;
    }

    public class MercenaryManager
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
            states[chara] = new MercState() { Level = 1 };
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
        public Dictionary<BonusType, double> CalculateBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
            {
                if (Contains(hero))
                {
                    MercState state = GetState(hero);
                    MercData data   = StaticData.Mercs.GetMerc(hero);

                    foreach (MercPassiveData passive in data.Passives)
                    {
                        if (state.Level >= passive.UnlockLevel)
                        {
                            if (passive.Value < 1)
                            {
                                bonuses[passive.Type] = bonuses.Get(passive.Type, 0) + passive.Value;
                            }

                            else
                                bonuses[passive.Type] = bonuses.Get(passive.Type, 1) * passive.Value;
                        }
                    }
                }
            }

            return bonuses;
        }
    }
}