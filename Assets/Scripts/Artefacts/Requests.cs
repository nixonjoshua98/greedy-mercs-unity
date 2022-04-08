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

    // Bulk Upgrade Artefact

    public class BulkArtefactUpgradeRequest : IServerRequest
    {
        public List<Models.BulkArtefactUpgrade> Artefacts;
    }

    public class BulkArtefactUpgradeResponse : ServerResponse
    {
        public double UpgradeCost;

        public double RemainingPrestigePoints;

        public List<Models.ArtefactUserDataModel> Artefacts;
    }
}
