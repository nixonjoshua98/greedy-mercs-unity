﻿using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;

using SimpleJSON;

namespace GM.Artefacts
{
    using GreedyMercs;
    using GM.Inventory;

    public class ArtefactState
    {
        public int ID;

        public int Level;

        // Quick dirty reference to the server 'static' data for the artefact
        ArtefactData _svrData { get { return StaticData.Artefacts.Get(ID); } }

        public ArtefactState(int id)
        {
            ID = id;
        }

        public bool IsMaxLevel() { return Level >= _svrData.MaxLevel; }

        // Formula shortcut
        public BigInteger CostToUpgrade(int levels)
        {
            return Formulas.ArtefactLevelUpCost(Level, levels, _svrData.costExpo, _svrData.costCoeff);
        }

        // Formula shortcut
        public double Effect()
        {
            return Formulas.ArtefactEffect(Level, _svrData.baseEffect, _svrData.levelEffect);
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

        // = = = Get = = = //
        public List<ArtefactState> StatesList { get { return states.Values.OrderBy(ele => ele.ID).ToList(); } }
        public int Count { get { return states.Count; } }

        public ArtefactState Get(int loot)
        {
            return states[loot];
        }

        // = = = Server Methods = = = //
        public void UpgradeArtefact(int artefactId, int levelsBuying, UnityAction<bool> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = GreedyMercs.Utils.Json.Decompress(body);

                    OnAnyServerRequestCallback(resp);
                }

                call.Invoke(code == 200);
            }

            JSONNode node = new JSONObject();

            node["artefactId"] = artefactId;
            node["totalLevelsBuying"] = levelsBuying;

            Server.Put("artefacts", "upgradeArtefact", node, Callback);
        }

        public void PurchaseNewArtefact(UnityAction<bool> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = GreedyMercs.Utils.Json.Decompress(body);

                    OnAnyServerRequestCallback(resp);
                }

                call.Invoke(code == 200);
            }

            JSONNode node = GreedyMercs.Utils.Json.GetDeviceInfo();

            Server.Put("artefacts", "purchaseArtefact", node, Callback);
        }

        // = = =

        public Dictionary<BonusType, double> CalculateBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            foreach (ArtefactState state in StatesList)
            {
                ArtefactData data = StaticData.Artefacts.Get(state.ID);

                double effect = state.Effect();

                switch (data.valueType)
                {
                    case ValueType.MULTIPLY:
                        bonuses[data.bonusType] = bonuses.Get(data.bonusType, 1) * effect;
                        break;

                    default:
                        bonuses[data.bonusType] = bonuses.Get(data.bonusType, 0) + effect;
                        break;
                }
            }

            return bonuses;
        }

        // = = = Internal Methods = = =
        void SetArtefactStates(JSONNode node)
        {
            states = new Dictionary<int, ArtefactState>();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                int id = int.Parse(key);

                ArtefactState state = new ArtefactState(id)
                {
                    Level = current["level"].AsInt
                };

                states[id] = state;
            }
        }

        // = = = Server Callbacks = = =
        void OnAnyServerRequestCallback(JSONNode node)
        {
            SetArtefactStates(node["userArtefacts"]);

            InventoryManager.Instance.SetItems(node["userItems"]);
        }
    }
}