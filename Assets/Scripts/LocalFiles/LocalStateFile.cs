using GM.Mercs.Data;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GM
{
    public class LocalStateFile
    {
        private const string FilePath = "LocalStateFile";

        [JsonProperty]
        public GameState GameState = new();

        [JsonProperty]
        public List<UserMercLocalState> MercStates { get; set; } = new();

        /* Inventory */
        public BigDouble Gold = 0;

        public void Clear(bool overwrite = false)
        {
            Gold = 0;
            GameState = new();
            MercStates.Clear();

            if (overwrite)
            {
                WriteToFile();
            }
        }

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
