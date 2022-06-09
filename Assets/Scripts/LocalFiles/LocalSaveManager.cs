namespace GM
{
    public class LocalSaveManager : GM.Core.GMMonoBehaviour
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
                App.SaveLocalStateFile();
            }

            App.PersistantLocalFile.WriteToFile();
        }
    }
}
