using GM.Mercs.Data;
using System.Collections.Generic;

namespace GM
{
    public class LocalStateFile
    {
        const string FilePath = "LocalStateFile";

        public CurrentPrestigeState GameState = new CurrentPrestigeState();
        public List<UserMercState> Mercs = new List<UserMercState>();

        public static FileStatus LoadFromFile(out LocalStateFile file)
        {
            return FileUtils.LoadModel(FilePath, out file);
        }

        public void WriteToFile()
        {
            FileUtils.WriteModel(FilePath, this);
        }

        public static void DeleteFile()
        {
            FileUtils.DeleteFile(FilePath);
        }
    }
}
