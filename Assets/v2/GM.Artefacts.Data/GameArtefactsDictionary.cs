using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Artefacts.Data
{
    /// <summary>
    /// Dictionary which stores artefacts game values
    /// </summary>
    public class GameArtefactsDictionary : Dictionary<int, ArtefactGameData>
    {
        public GameArtefactsDictionary(JSONNode node)
        {
            UpdateFromJSON(node);
        }


        /// <summary>
        /// Update the dictionary using a JSON (most likely from the server)
        /// </summary>
        /// <param name="node">JSON</param>
        public void UpdateFromJSON(JSONNode node)
        {
            Clear();

            foreach (LocalArtefactData local in LoadLocalData())
            {
                if (node.TryGetKey(local.ID, out JSONNode current))
                {
                    base[local.ID] = new ArtefactGameData()
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


        static LocalArtefactData[] LoadLocalData() => Resources.LoadAll<LocalArtefactData>("Artefacts");
    }
}
