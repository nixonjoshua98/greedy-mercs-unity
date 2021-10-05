using SimpleJSON;
using GM.Artefacts.Models;
using System.Collections.Generic;

namespace GM.Artefacts.Data
{
    /// <summary>
    /// Dictionary which stores user artefacta data
    /// </summary>
    public class UserArtefactsCollection : Dictionary<int, UserArtefactModel>
    {
        public UserArtefactsCollection(JSONNode node)
        {
            UpdateFromJSON(node);
        }


        /// <summary>
        /// Update the dictionary using a JSON (most likely from the server)
        /// </summary>
        /// <param name="node">JSON</param>
        public void UpdateFromJSON(JSONNode node)
        {
            Clear();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                int id = int.Parse(key);

                var state = new UserArtefactModel()
                {
                    Id=id,
                    Level = current["level"].AsInt
                };

                base[id] = state;
            }
        }
        
        public void Update(List<UserArtefactModel> ls)
        {
            Clear();

            foreach (var art in ls)
            {
                base[art.Id] = art;
            }
        }
    }
}
