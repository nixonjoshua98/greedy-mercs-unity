using GM.Mercs.Data;
using System.Collections.Generic;

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

        public static FileStatus LoadFromFile(out LocalStateFile file)
        {
            return FileUtils.LoadModel(LocalSaveManager.LOCAL_FILE, out file);
        }
    }
}
