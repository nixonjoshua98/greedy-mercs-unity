using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using GM.Artefacts.Models;
using GM.Extensions;
using System.Linq;
namespace GM.Artefacts.Data
{
    /// <summary>
    /// Dictionary which stores artefacts game values
    /// </summary>
    public class GameArtefactsCollection
    {
        List<ArtefactGameDataModel> gameArtefactsList;

        public int Count => gameArtefactsList.Count;

        public GameArtefactsCollection(List<ArtefactGameDataModel> artefacts)
        {
            Update(artefacts);
        }

        public GameArtefactsCollection(JSONNode node)
        {
            UpdateFromJSON(node);
        }

        public ArtefactGameDataModel Get(int key) => gameArtefactsList.Where(art => art.Id == key).FirstOrDefault();

        void Update(List<ArtefactGameDataModel> artefacts)
        {
            gameArtefactsList = artefacts;

            LocalArtefactData[] allLocalMercData = LoadLocalData();

            foreach (var art in gameArtefactsList)
            {
                LocalArtefactData localArtData = allLocalMercData.Where(ele => ele.Id == art.Id).First();

                art.Name = localArtData.Name;
                art.Icon = localArtData.Icon;
                art.Slot = localArtData.Slot;
            }
        }


        /// <summary>
        /// Update the dictionary using a JSON (most likely from the server)
        /// </summary>
        /// <param name="node">JSON</param>
        public void UpdateFromJSON(JSONNode node)
        {
            //Clear();

            //foreach (LocalArtefactData local in LoadLocalData())
            //{
            //    if (node.TryGetKey(local.Id, out JSONNode current))
            //    {
            //        base[local.Id] = new Models.ArtefactGameDataModel()
            //        {
            //            Id = local.Id,
            //            Name = local.Name,
            //            Icon = local.Icon,
            //            Slot = local.Slot,

            //            Bonus = (BonusType)current["bonusType"].AsInt,
            //            MaxLevel = current.GetValueOrDefault("maxLevel", 1_000).AsInt,

            //            CostExpo = current["costExpo"],
            //            CostCoeff = current["costCoeff"],
            //            BaseEffect = current["baseEffect"],
            //            LevelEffect = current["levelEffect"]
            //        };
            //    }
            //}
        }


        static LocalArtefactData[] LoadLocalData() => Resources.LoadAll<LocalArtefactData>("Artefacts");
    }
}
