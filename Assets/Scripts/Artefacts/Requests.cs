using GM.HTTP;
using System.Collections.Generic;

namespace GM.Artefacts
{
    // Unlock Artefact

    public class UnlockArtefactResponse : ServerResponse
    {
        public Models.ArtefactUserDataModel Artefact;

        public double UnlockCost;
    }

    public class BulkArtefactUpgradeResponse : ServerResponse
    {
        public double UpgradeCost;

        public double RemainingPrestigePoints;

        public List<Models.ArtefactUserDataModel> Artefacts;
    }
}
