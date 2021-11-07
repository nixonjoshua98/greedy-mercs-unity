using Newtonsoft.Json;

namespace GM.Artefacts.Models
{
    public class ArtefactUserDataModel
    {
        [JsonProperty(PropertyName = "artefactId")]
        public int Id;

        [JsonProperty(PropertyName = "level")]
        public int ActualLevel;

        [JsonIgnore]
        public int DummyLevel;

        [JsonIgnore]
        public int Level => ActualLevel + DummyLevel;
    }
}
