namespace GM.HTTP.Requests
{
    public class UpgradeArtefactRequest : AuthorisedServerRequest
    {
        public int ArtefactId;
        public int UpgradeLevels;
    }

    public class UpgradeArtefactResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel UserCurrencies;
        public Artefacts.Models.UserArtefactModel UpdatedArtefact;
    }
}