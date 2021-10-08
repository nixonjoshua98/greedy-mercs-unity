using GM.Artefacts.Models;
using GM.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace GM.Artefacts.Data
{
    public class UserArtefactsCollection
    {
        List<UserArtefactModel> userArtefactsList;

        public List<UserArtefactModel> List => userArtefactsList;

        public int Count => userArtefactsList.Count;
        
        public UserArtefactsCollection(List<UserArtefactModel> artefacts)
        {
            userArtefactsList = artefacts;
        }

        public UserArtefactModel Get(int key) => userArtefactsList.Where(art => art.Id == key).FirstOrDefault();

        public void Update(UserArtefactModel art)
        {
            userArtefactsList.UpdateOrInsertElement(art, (ele) => ele.Id == art.Id);
        }
    }
}
