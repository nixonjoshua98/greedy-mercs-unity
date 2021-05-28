using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Artefacts
{
    using GreedyMercs;

    public class ArtefactState
    {
        public int ID;

        public int Level;

        public ArtefactState(int id)
        {
            ID = id;
        }
    }

    public class ArtefactManager
    {
        public static ArtefactManager Instance = null;

        Dictionary<int, ArtefactState> states;

        public static ArtefactManager Create(JSONNode node)
        {
            Instance = new ArtefactManager(node);

            return Instance;
        }

        ArtefactManager(JSONNode node)
        {
            SetArtefactStates(node);
        }

        public void SetArtefactStates(JSONNode node)
        {
            states = new Dictionary<int, ArtefactState>();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                int id = int.Parse(key);

                ArtefactState state = new ArtefactState(id)
                {
                    Level = current.AsInt
                };

                states[id] = state;
            }
        }

        // = = = Get = = =
        public List<ArtefactState> StatesList { get { return states.Values.ToList(); } }
        public int Count { get { return states.Count; } }

        public ArtefactState Get(int loot)
        {
            return states[loot];
        }

        public void Temp_Add(int artefactId)
        {
            states[artefactId] = new ArtefactState(artefactId) { Level = 1 };
        }

        // = = =

        public Dictionary<BonusType, double> CalculateTotalBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            foreach (ArtefactState state in StatesList)
            {
                ArtefactData data = StaticData.Artefacts.Get(state.ID);

                switch (data.valueType)
                {
                    case ValueType.MULTIPLY:
                        bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 1) * Formulas.CalcLootItemEffect(state.ID);
                        break;

                    case ValueType.ADDITIVE_FLAT_VAL:
                        bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcLootItemEffect(state.ID);
                        break;

                    case ValueType.ADDITIVE_PERCENT:
                        bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcLootItemEffect(state.ID);
                        break;
                }
            }

            return bonuses;
        }
    }
}