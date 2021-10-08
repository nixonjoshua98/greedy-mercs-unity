namespace GM.HTTP.Requests
{
    public class UnlockArtefactResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Artefacts.Models.UserArtefactModel NewArtefact;
    }
}