﻿namespace GM.HTTP.Requests
{
    public class UpgradeArtefactRequest : AuthorisedRequest
    {
        public int ArtefactId;
        public int UpgradeLevels;
    }

    public class UpgradeArtefactResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Artefacts.Models.UserArtefactModel UpdatedArtefact;
    }
}