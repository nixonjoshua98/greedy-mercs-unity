using Newtonsoft.Json;

namespace GM.Artefacts.Models
{
    public class ArtefactUserDataModel
    {
        [JsonProperty(PropertyName = "ArtefactID")]
        public int ID { get; set; }

        public int Level { get; set; }
    }
}