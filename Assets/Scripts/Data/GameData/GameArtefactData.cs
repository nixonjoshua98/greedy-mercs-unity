using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GM.Artefacts
{
    public struct ArtefactData
    {
        public int ID;

        public string Name;

        public BonusType Bonus;

        public int MaxLevel;

        public float CostExpo;
        public float CostCoeff;
        public float BaseEffect;
        public float LevelEffect;

        public Sprite Icon;
        public ArtefactSlot Slot;
    }


    public class GameArtefactData
    {
        Dictionary<int, ArtefactData> artefacts;

        public int Count { get { return artefacts.Count; } }

        public GameArtefactData(JSONNode node)
        {
            UpdateData(node);
        }

        // = = = Public Methods = = = //
        public ArtefactData Get(int id) => artefacts[id];


        void UpdateData(JSONNode node)
        {
            artefacts = new Dictionary<int, ArtefactData>();

            foreach (LocalArtefactData local in LoadLocalData())
            {
                if (node.TryGetKey(local.ID, out JSONNode current))
                {
                    artefacts[local.ID] = new ArtefactData()
                    {
                        ID = local.ID,
                        Name = local.Name,
                        Icon = local.Icon,
                        Slot = local.Slot,

                        Bonus = (BonusType)current["bonusType"].AsInt,
                        MaxLevel = current.GetValueOrDefault("maxLevel", 1_000).AsInt,

                        CostExpo = current["costExpo"],
                        CostCoeff = current["costCoeff"],
                        BaseEffect = current["baseEffect"],
                        LevelEffect = current["levelEffect"]
                    };
                }
            }
        }


        LocalArtefactData[] LoadLocalData() => Resources.LoadAll<LocalArtefactData>("Artefacts");
    }
}
