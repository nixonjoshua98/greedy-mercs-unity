using GM.Common.Json;
using Newtonsoft.Json;
using BigInteger = System.Numerics.BigInteger;
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

        public Inventory.Models.UserCurrencies CurrencyItems;
        public Artefacts.Models.ArtefactUserDataModel Artefact;
    }
}