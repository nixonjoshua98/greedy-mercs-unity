﻿using Newtonsoft.Json;
using SRC.Common;
using SRC.Mercs.Data;
using System.Collections.Generic;

namespace SRC
{
    public class LocalStateFile
    {
        private const string FilePath = "LocalStateFile";

        [JsonProperty]
        public GameState GameState = new();

        [JsonProperty]
        public List<MercLocalUserData> MercStates { get; set; } = new();

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
            return FileIO.LoadModel(FilePath, out file);
        }

        public void WriteToFile()
        {
            FileIO.WriteModel(FilePath, this);
        }

        public static void DeleteFile()
        {
            FileIO.DeleteFile(FilePath);
        }
    }
}
