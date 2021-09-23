using System.Collections.Generic;
using System.Linq;

namespace GM.Mercs.Data
{
    public class MercUserDataDictionary : Dictionary<MercID, MercUserData>
    {
        public List<MercID> Unlocked => this.Where(pair => pair.Value.Level > 0).Select(pair => pair.Key).ToList();
    }
}
