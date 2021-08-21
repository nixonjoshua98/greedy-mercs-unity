using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace GM.Data
{
    public interface ILocalSave
    {
        string FilePath { get; set; }

        void Save();
    }


    public class LocalSaveJSON : ILocalSave
    {
        public string FilePath { get; set; }

        public JSONNode Node;

        public void Save()
        {
            Debug.Log(FilePath);

            FileUtils.WriteJSON(FilePath, Node);
        }
    }


    public interface ILocalDataContainer
    {
        ILocalSave GetLocalSaveData();
    }

}
