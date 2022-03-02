using System.Collections;
using UnityEngine;
using GM.Common.Interfaces;

namespace GM
{
    public class LocalSaveManager : Common.MonoClass<LocalSaveManager>
    {
        const string STATIC_FILE = "staticdata";
        const string LOCAL_FILE = "localsave";

        public bool Paused { get; set; } = false;

        void Awake()
        {
            StartCoroutine(SaveLoop());
        }

        IEnumerator SaveLoop()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(3);

                if (!Paused)
                {
                    Save();
                }
            }
        }

        public void Save()
        {
            // = Models = //
            var savefile = GMData.CreateLocalSaveFile();

            // = Write to File = //
            FileUtils.WriteModel(LOCAL_FILE, savefile);
        }

        public void WriteStaticData(IStaticGameData model) => FileUtils.WriteModel(STATIC_FILE, model);

        public void DeleteLocalFile() => FileUtils.DeleteFile(LOCAL_FILE);

        public LocalSaveFileModel LoadSaveFile()
        {
            FileStatus status = FileUtils.LoadModel(LOCAL_FILE, out LocalSaveFileModel model);

            switch (status)
            {
                case FileStatus.OK:
                    return model;

                case FileStatus.CORRUPTED:
                    Debug.Log("Local save file was corrupted");
                    return new LocalSaveFileModel();

                default:
                    return new LocalSaveFileModel();
            }
        }
    }
}
