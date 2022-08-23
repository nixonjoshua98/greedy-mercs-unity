using Newtonsoft.Json;

namespace SRC.Artefacts.Data
{
    public class UserArtefact
    {
        [JsonProperty(PropertyName = "ArtefactID")]
        public int ID;

        public int Level;
    }
}