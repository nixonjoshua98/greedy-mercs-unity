namespace GM.HTTP.Requests
{
    public class UnlockArtefactResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel UserCurrencies;
        public Artefacts.Models.UserArtefactModel NewArtefact;
    }
}