using Newtonsoft.Json;
using BigInteger = System.Numerics.BigInteger;

namespace GM.HTTP.Requests
{
    public class UnlockArtefactResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Artefacts.Models.ArtefactUserDataModel Artefact;

        [JsonConverter(typeof(Common.Json.BigIntegerConverter))]
        public BigInteger UnlockCost;
    }
}