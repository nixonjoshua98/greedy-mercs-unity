using SimpleJSON;
using System.Collections.Generic;

namespace GM.Data
{
    public class UserArtefactDictionary : Dictionary<int, ArtefactState>
    {
        public UserArtefactDictionary(JSONNode node)
        {
            UpdateFromJSON(node);
        }


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
