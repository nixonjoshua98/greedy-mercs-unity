using System.Collections.Generic;

namespace SRC.Artefacts.Requests
{
    public class UnlockArtefactResponse : SRC.HTTP.Requests.ServerResponse
    {
        public Data.ArtefactUserData Artefact;

        public double UnlockCost;
    }

    public class BulkArtefactUpgradeResponse : SRC.HTTP.Requests.ServerResponse
    {
        public double UpgradeCost;

        public double RemainingPrestigePoints;

        public List<Data.ArtefactUserData> Artefacts;
    }
}
