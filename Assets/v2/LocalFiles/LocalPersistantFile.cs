using GM.Common.Enums;
using GM.PlayerStats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GM.LocalFiles
{
    public sealed class LocalPersistantFile : GM.Core.GMClass
    {
        private const string FilePath = "PersistantLocalFile";

        [JsonProperty]
        private TimedPlayerStatsModel _LocalDailyStats;

        [JsonProperty]
        public HashSet<MercID> SquadMercIDs { get; set; } = new();

        [JsonIgnore]
        public TimedPlayerStatsModel LocalDailyStats
        {
            get
            {
                if (_LocalDailyStats is null)
                    _LocalDailyStats = new();

                else if (!_LocalDailyStats.DateTime.IsBetween(App.DailyRefresh.PreviousNextReset))
                {
                    _LocalDailyStats = new() { DateTime = DateTime.UtcNow };
                }

                return _LocalDailyStats;
            }
            set => _LocalDailyStats = value;
        }

        [JsonProperty]
        public LifetimeStatsModel LocalLifetimeStats { get; set; } = new();

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
