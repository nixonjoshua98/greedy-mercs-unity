using BigInteger = System.Numerics.BigInteger;
using GM.Common.Json;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace GM.HTTP.Requests
{
    public class BulkUpgradeRequest : IServerRequest
    {
        public List<GM.Artefacts.Models.BulkArtefactUpgrade> Artefacts;
    }

    public class BulkUpgradeResponse : ServerResponse
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger UpgradeCost;

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger PrestigePoints;

        public List<Artefacts.Models.ArtefactUserDataModel> Artefacts;
    }
}