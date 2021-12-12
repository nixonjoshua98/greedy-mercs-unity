using Newtonsoft.Json;

namespace GM.Artefacts.Models
{
    public class ArtefactUserDataModel
    {
        [JsonProperty(PropertyName = "artefactId")]
        [JsonRequired]
        public int Id;

        [JsonProperty]
        [JsonRequired]
        public int Level;
    }
}