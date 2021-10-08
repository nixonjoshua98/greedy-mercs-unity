using GM.Artefacts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Artefacts.Data
{
    public class GameArtefactsCollection
    {
        List<ArtefactGameDataModel> gameArtefactsList;

        public int Count => gameArtefactsList.Count;

        public GameArtefactsCollection(List<ArtefactGameDataModel> artefacts)
        {
            Update(artefacts);
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

        static LocalArtefactData[] LoadLocalData() => Resources.LoadAll<LocalArtefactData>("Artefacts");
    }
}
