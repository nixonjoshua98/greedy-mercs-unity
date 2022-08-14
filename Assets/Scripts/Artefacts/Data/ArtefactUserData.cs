using Newtonsoft.Json;

namespace GM.Artefacts.Data
{
    public class ArtefactUserData
    {
        [JsonProperty(PropertyName = "ArtefactID")]
        public int ID;

        public int Level;
    }
}