using GM.Common.Enums;
using GM.PlayerStats;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace GM.LocalFiles
{
    public sealed class LocalPersistantFile : GM.Core.GMClass
    {
        const string FilePath = "PersistantLocalFile";

        [JsonProperty]
        private TimedStatsModel _LocalDailyStats;

        [JsonProperty]
        public HashSet<MercID> SquadMercIDs { get; set; } = new HashSet<MercID>();

        [JsonIgnore]
        public TimedStatsModel LocalDailyStats {
            get
            {
                if (_LocalDailyStats is null)
                    _LocalDailyStats = new();

                else if (!_LocalDailyStats.CreatedTime.IsBetween(App.DailyRefresh.PreviousNextReset))
                {
                    GMLogger.Editor("Reset LocalDailyStats");
                    _LocalDailyStats = new() { CreatedTime = DateTime.UtcNow };
                }

                return _LocalDailyStats;
            }
            set => _LocalDailyStats = value;
        }

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
