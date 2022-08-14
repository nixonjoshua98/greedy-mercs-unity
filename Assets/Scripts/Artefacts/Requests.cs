using System.Collections.Generic;

namespace GM.Artefacts.Requests
{
    public class UnlockArtefactResponse : GM.HTTP.Requests.ServerResponse
    {
        public Data.ArtefactUserData Artefact;

        public double UnlockCost;
    }

    public class BulkArtefactUpgradeResponse : GM.HTTP.Requests.ServerResponse
    {
        public double UpgradeCost;

        public double RemainingPrestigePoints;

        public List<Data.ArtefactUserData> Artefacts;
    }
}
