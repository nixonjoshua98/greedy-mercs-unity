using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UnlockArtefactResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel UserCurrencies;
        public List<GM.Artefacts.Models.UserArtefactModel> UserArtefacts;
        public int NewArtefactId;
    }
}