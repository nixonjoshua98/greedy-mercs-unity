using SimpleJSON;
using System.Collections.Generic;

namespace GM.Artefacts.Data
{
    /// <summary>
    /// DIctionary which stores user artefacta data
    /// </summary>
    public class UserArtefactsDictionary : Dictionary<int, ArtefactState>
    {
        public UserArtefactsDictionary(JSONNode node)
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

                ArtefactState state = new ArtefactState(id)
                {
                    Level = current["level"].AsInt
                };

                base[id] = state;
            }
        }
    }
}
