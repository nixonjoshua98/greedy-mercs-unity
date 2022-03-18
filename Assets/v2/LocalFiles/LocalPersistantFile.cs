using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.LocalFiles
{
    public sealed class LocalPersistantFile
    {
        const string FilePath = "PersistantLocalFile";

        // Serialized Fields
        [JsonProperty] public HashSet<MercID> SquadMercIDs { get; set; } = new HashSet<MercID>();
        // ...

        /// <summary>
        /// Static constructor (preferred usage)
        /// </summary>
        public static FileStatus LoadFromFile(out LocalPersistantFile file)
        {
            FileStatus status = FileUtils.LoadModel(FilePath, out file);

            return status;
        }

        /// <summary>
        /// Write the model to file
        /// </summary>
        public void WriteToFile()
        {
            FileUtils.WriteModel(FilePath, this);
        }
    }
}
