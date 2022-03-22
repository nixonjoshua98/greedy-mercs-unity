using System.Collections;
using UnityEngine;

namespace GM
{
    public class LocalSaveManager : Common.MonoBehaviourLazySingleton<LocalSaveManager>
    {
        public bool Paused { get; set; } = false;

        void Awake()
        {
            StartCoroutine(SaveLoop());
        }

        IEnumerator SaveLoop()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);

                if (!Paused)
                {
                    App.SaveLocalStateFile();
                }

                App.PersistantLocalFile.WriteToFile();
            }
        }
    }
}
