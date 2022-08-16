using Newtonsoft.Json;

namespace SRC.Artefacts.Data
{
    public class ArtefactUserData
    {
        [JsonProperty(PropertyName = "ArtefactID")]
        public int ID;

        public int Level;
    }
}