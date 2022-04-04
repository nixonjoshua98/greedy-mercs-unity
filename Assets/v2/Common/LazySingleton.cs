namespace GM.Common
{
    public abstract class LazySingleton<T> : Core.GMClass where T : new()
    {
        private static T s_Instance;

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new T();
                }

                return s_Instance;
            }
        }
    }
}