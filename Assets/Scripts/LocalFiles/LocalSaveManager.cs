namespace SRC
{
    public class LocalSaveManager : SRC.Core.GMMonoBehaviour
    {
        public bool Paused { get; set; } = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            InvokeRepeating(nameof(_PeriodicUpdate), 3.0f, 1.0f);
        }

        private void _PeriodicUpdate()
        {
            if (!Paused)
            {
                App.LocalStateFile.WriteToFile();
            }

            App.PersistantLocalFile.WriteToFile();
        }
    }
}
