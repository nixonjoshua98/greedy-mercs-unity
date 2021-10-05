using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UpgradeArtefactRequest : AuthorisedServerRequest
    {
        public int ArtefactId;
        public int UpgradeLevels;
    }

    public class UpgradeArtefactResponse : ServerResponse
    {
        public GM.Inventory.Models.UserCurrenciesModel UserCurrencies;
        public List<GM.Artefacts.Models.UserArtefactModel> UserArtefacts;
    }
}