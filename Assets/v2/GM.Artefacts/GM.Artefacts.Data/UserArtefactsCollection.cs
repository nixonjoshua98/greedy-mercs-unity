using SimpleJSON;
using GM.Artefacts.Models;
using GM.Extensions;
using System.Collections.Generic;
using System.Linq;
namespace GM.Artefacts.Data
{
    /// <summary>
    /// Dictionary which stores user artefacta data
    /// </summary>
    public class UserArtefactsCollection
    {
        List<UserArtefactModel> userArtefactsList;

        public List<UserArtefactModel> List => userArtefactsList;

        public int Count => userArtefactsList.Count;

        public UserArtefactsCollection(JSONNode node)
        {
            UpdateFromJSON(node);
        }

        public UserArtefactsCollection(List<UserArtefactModel> artefacts)
        {
            userArtefactsList = artefacts;
        }

        public UserArtefactModel Get(int key) => userArtefactsList.Where(art => art.Id == key).FirstOrDefault();



        /// <summary>
        /// Update the dictionary using a JSON (most likely from the server)
        /// </summary>
        /// <param name="node">JSON</param>
        public void UpdateFromJSON(JSONNode node)
        {
            //Clear();

            //foreach (string key in node.Keys)
            //{
            //    JSONNode current = node[key];

            //    int id = int.Parse(key);

            //    var state = new UserArtefactModel()
            //    {
            //        Id=id,
            //        Level = current["level"].AsInt
            //    };

            //    base[id] = state;
            //}
        }
        
        public void Update(List<UserArtefactModel> ls)
        {
            userArtefactsList = ls;
        }

        public void Update(UserArtefactModel art)
        {
            userArtefactsList.UpdateOrInsertElement(art, (ele) => ele.Id == art.Id);
        }
    }
}
