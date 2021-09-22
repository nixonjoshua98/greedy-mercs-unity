using SimpleJSON;
using System.Collections.Generic;

namespace GM.Data
{
    public class UserArtefactsDictionary : Dictionary<int, ArtefactState>
    {
        public UserArtefactsDictionary(JSONNode node)
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
