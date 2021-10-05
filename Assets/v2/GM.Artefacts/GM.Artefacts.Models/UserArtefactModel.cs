using Newtonsoft.Json;

namespace GM.Artefacts.Models
{
    public class UserArtefactModel
    {
        [JsonProperty(PropertyName = "artefactId")]
        public int Id;

        public int Level;
    }
}
