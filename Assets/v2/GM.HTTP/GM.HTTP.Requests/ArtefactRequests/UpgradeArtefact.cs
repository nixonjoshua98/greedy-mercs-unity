using BigInteger = System.Numerics.BigInteger;
using GM.Common.Json;
using Newtonsoft.Json;
namespace GM.HTTP.Requests
{
    public class UpgradeArtefactRequest : IServerRequest
    {
        public int ArtefactId;
        public int UpgradeLevels;
    }

    public class UpgradeArtefactResponse : ServerResponse
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger UpgradeCost;

        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Artefacts.Models.ArtefactUserDataModel UpdatedArtefact;
    }
}