using GM.Common.Enums;
using GM.PlayerStats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GM.LocalFiles
{
    public sealed class LocalPersistantFile : GM.Core.GMClass
    {
        private const string FilePath = "LocalPersistantFile";

        [JsonProperty]
        private TimedPlayerStatsModel _LocalDailyStats;

        [JsonProperty]
        public HashSet<MercID> SquadMercIDs = new();

        [JsonIgnore]
        public TimedPlayerStatsModel LocalDailyStats
        {
            get
            {
                _LocalDailyStats ??= new();

                // Clear outdated data
                if (!App.DailyRefresh.Current.IsBetween(_LocalDailyStats.LastUpdated))
                    _LocalDailyStats = new() { LastUpdated = DateTime.UtcNow };

                return _LocalDailyStats;
            }
            set => _LocalDailyStats = value;
        }

        [JsonProperty]
        public LifetimeStatsModel LocalLifetimeStats = new();

        /// <summary>
        /// Static constructor (preferred usage)
        /// </summary>
        public static FileStatus LoadFromFile(out LocalPersistantFile file)
        {
            return FileUtils.LoadModel(FilePath, out file);
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
