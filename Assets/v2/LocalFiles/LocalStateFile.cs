using System.Collections.Generic;
using GM.Mercs.Data;

namespace GM
{
    interface ILocalStateFileSerializer
    {
        void UpdateLocalSaveFile(ref LocalStateFile model);
    }

    public class LocalStateFile
    {
        public CurrentPrestigeState GameState = new CurrentPrestigeState();
        public List<UserMercState> Mercs = new List<UserMercState>();
    }
}
