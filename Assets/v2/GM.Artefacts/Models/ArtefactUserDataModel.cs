using Newtonsoft.Json;

namespace GM.Artefacts.Models
{
    public class ArtefactUserDataModel
    {
        [JsonProperty(PropertyName = "artefactId")]
        public int Id;

        [JsonProperty]
        public int Level;
    }
}
