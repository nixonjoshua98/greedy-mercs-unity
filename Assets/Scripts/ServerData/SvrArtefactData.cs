using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GM.Artefacts
{
    using BonusType = GreedyMercs.BonusType;

    public enum ValueType
    {
        MULTIPLY = 0,
        ADDITIVE_PERCENT = 1,
        ADDITIVE_FLAT_VAL = 2
    }

    public class ArtefactData
    {
        public int ID;

        public string Name;

        public BonusType bonusType;
        public ValueType valueType;

        public int MaxLevel;

        public float costExpo;
        public float costCoeff;
        public float baseEffect;
        public float levelEffect;

        string iconString;

        public ArtefactData(int id, JSONNode node)
        {
            ID = id;

            Name = node.GetValueOrDefault("artefactName", "Artefact Name");

            bonusType = (BonusType)node["bonusType"].AsInt;
            valueType = (ValueType)node["valueType"].AsInt;

            MaxLevel = node.GetValueOrDefault("maxLevel", 1_000).AsInt;

            costExpo    = node["costExpo"];
            costCoeff   = node["costCoeff"];

            baseEffect  = node["baseEffect"];
            levelEffect = node["levelEffect"];

            iconString = node.GetValueOrDefault("iconString", "ArtefactIcons/apple");
        }

        public Sprite Icon { get { return ResourceManager.LoadSprite(iconString); } }
    }

    public class SvrArtefactData
    {
        Dictionary<int, ArtefactData> artefacts;

        public int Count { get { return artefacts.Count; } }

        public SvrArtefactData(JSONNode node)
        {
            SetData(node);
        }

        void SetData(JSONNode node)
        {
            artefacts = new Dictionary<int, ArtefactData>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                ArtefactData inst = new ArtefactData(id, node[key]);

                artefacts[id] = inst;
            }
        }

        // = = = Get = = =
        public ArtefactData Get(int id)
        {
            return artefacts[id];
        }
    }
}