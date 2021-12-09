using BigInteger = System.Numerics.BigInteger;
using GM.Common.Json;
using Newtonsoft.Json;
namespace GM.HTTP.Requests
{
    public class UpgradeArtefactRequest : IServerRequest
    {
        public readonly int ArtefactId;
        public readonly int UpgradeLevels;

        public UpgradeArtefactRequest(int artefact, int levels)
        {
            ArtefactId = artefact;
            UpgradeLevels = levels;
        }
    }

    public class UpgradeArtefactResponse : ServerResponse
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger UpgradeCost;

        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Artefacts.Models.ArtefactUserDataModel Artefact;
    }
}