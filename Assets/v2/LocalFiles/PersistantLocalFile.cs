using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.LocalFiles
{
    public sealed class PersistantLocalFile
    {
        const string FilePath = "PersistantLocalFile";

        // Serialised Fields
        [JsonProperty]
        public HashSet<UnitID> SquadMercIDs = new HashSet<UnitID>();
        // ...

        /// <summary>
        /// Static constructor (preferred usage)
        /// </summary>
        public static FileStatus LoadFromFile(out PersistantLocalFile file)
        {
            FileStatus status = FileUtils.LoadModel(FilePath, out file);

            file.Validate();

            return status;
        }

        /// <summary>
        /// Write the model to file
        /// </summary>
        public void WriteToFile()
        {
            FileUtils.WriteModel(FilePath, this);
        }

        /// <summary>
        /// Validate the save file model, resetting any invalid data
        /// </summary>
        void Validate()
        {
            Validate_SquadMercIDs();
        }

        /// <summary>
        /// Validate and fix the squad mercs
        /// </summary>
        void Validate_SquadMercIDs()
        {
            if (SquadMercIDs == null || SquadMercIDs.Count > GM.Common.Constants.MAX_SQUAD_SIZE)
                SquadMercIDs = new HashSet<UnitID>();
        }
    }
}
