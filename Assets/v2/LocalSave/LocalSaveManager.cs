using System.Collections;
using UnityEngine;
using GM.Common.Interfaces;

namespace GM.LocalSave
{
    public class LocalSaveManager : Common.MonoClass<LocalSaveManager>
    {
        const string STATIC_FILE = "staticdata";
        const string LOCAL_FILE = "localsave";
        const string USER_FILE = "userdata";

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
            var savefile = new LocalSaveFileModel();
            var userdata = new LocalUserDataFileModel();

            // = Update Models = //
            App.Data.GameState.Serialize(ref savefile);
            App.Data.Artefacts.Serialize(ref userdata);

            // = Write to File = //
            FileUtils.WriteModel(LOCAL_FILE, savefile);
            FileUtils.WriteModel(USER_FILE, userdata);
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
