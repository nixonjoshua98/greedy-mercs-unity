using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;

using SimpleJSON;

namespace GM.Artefacts
{
    using GM.Server;
    using GM.Data;

    public class ArtefactState
    {
        public readonly int ID;

        public int Level;

        public ArtefactState(int id)
        {
            ID = id;
        }

        ArtefactData _Data => GameData.Get.Artefacts.Get(ID);

        public bool IsMaxLevel() { return Level >= _Data.MaxLevel; }

        public BigInteger CostToUpgrade(int levels) => Formulas.ArtefactLevelUpCost(Level, levels, _Data.CostExpo, _Data.CostCoeff);

        public double Effect() => Formulas.BaseArtefactEffect(Level, _Data.BaseEffect, _Data.LevelEffect);
    }


    public class UserArtefacts
    {
        Dictionary<int, ArtefactState> states;


        public UserArtefacts(JSONNode node)
        {
            UpdateArtefactStates(node);
        }


        public ArtefactState[] StatesList => states.Values.OrderBy(ele => ele.ID).ToArray();
        public int Count => states.Count;
        public ArtefactState Get(int id) => states[id];


        // = = = Server Methods = = = //

        public void UpgradeArtefact(int artefactId, int levelsBuying, UnityAction<bool> call)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    OnServerResponse(resp);
                }

                call.Invoke(code == 200);
            }

            JSONNode node = new JSONObject();

            node["artefactId"] = artefactId;
            node["purchaseLevels"] = levelsBuying;

            HTTPClient.GetClient().Post("artefact/upgrade", node, Callback);
        }


        public void UnlockArtefact(UnityAction<bool, int> call)
        {
            void InternalCallback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    OnServerResponse(resp);
                }

                call.Invoke(code == 200, code == 200 ? resp["newArtefactId"].AsInt : -1);
            }

            HTTPClient.GetClient().Post("artefact/unlock", InternalCallback);
        }
        // = = = ^


        // = = = Server Callbacks = = = //
        void OnServerResponse(JSONNode node)
        {
            UpdateArtefactStates(node["userArtefacts"]);

            UserData.Get.Inventory.SetServerItemData(node["userItems"]);
        }
        // = = = ^


        // = = = Private = = = //
        void UpdateArtefactStates(JSONNode node)
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

        // = = = Public = = = //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (ArtefactState state in StatesList)
            {
                ArtefactData data = GameData.Get.Artefacts.Get(state.ID);

                ls.Add(new KeyValuePair<BonusType, double>(data.Bonus, state.Effect()));
            }

            return ls;
        }
        // = = = ^
    }
}
