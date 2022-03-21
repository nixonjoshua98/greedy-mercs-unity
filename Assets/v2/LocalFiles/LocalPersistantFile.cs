﻿using GM.Common.Enums;
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
        private DailyStatsModel _LocalDailyStats;

        [JsonProperty]
        public HashSet<MercID> SquadMercIDs { get; set; } = new HashSet<MercID>();

        [JsonIgnore]
        public DailyStatsModel LocalDailyStats {
            get
            {
                if (_LocalDailyStats is null)
                    _LocalDailyStats = new();

                else if (DateTime.UtcNow >= _LocalDailyStats.NextRefresh)
                {
                    _LocalDailyStats = new() { NextRefresh = App.DailyRefresh.Next };
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
