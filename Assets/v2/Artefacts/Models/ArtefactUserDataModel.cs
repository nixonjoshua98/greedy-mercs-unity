using Newtonsoft.Json;

namespace GM.Artefacts.Models
{
    public class ArtefactUserDataModel
    {
        [JsonProperty(PropertyName = "artefactId", Required = Required.Always)]
        public int Id;

        [JsonProperty(Required = Required.Always)]
        public int Level { get; set; }
    }
}