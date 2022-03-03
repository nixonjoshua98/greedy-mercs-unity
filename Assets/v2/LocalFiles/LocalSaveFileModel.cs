using System.Collections.Generic;
using GM.Mercs.Data;

namespace GM
{
    public class LocalSaveFileModel
    {
        public CurrentPrestigeState GameState = new CurrentPrestigeState();
        public List<UserMercState> Mercs = new List<UserMercState>();
    }
}
