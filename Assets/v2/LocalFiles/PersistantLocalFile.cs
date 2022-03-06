using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.LocalFiles
{
    public interface IPersistantLocalFileValidator
    {
        void ValidatePersistantLocalFile(ref PersistantLocalFile file);
    }


    public sealed class PersistantLocalFile
    {
        const string FilePath = "PersistantLocalFile";

        // Serialized Fields
        [JsonProperty]
        public HashSet<UnitID> SquadMercIDs = new HashSet<UnitID>();
        // ...

        /// <summary>
        /// Static constructor (preferred usage)
        /// </summary>
        public static FileStatus LoadFromFile(out PersistantLocalFile file)
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
