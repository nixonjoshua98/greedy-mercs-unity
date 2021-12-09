namespace GM.Common
{
    public abstract class LazySingleton<T> : Core.GMClass where T: new()
    {
        static object s_Lock = new object();

        static T s_Instance;

        public static T Instance
        {
            get
            {
                lock (s_Lock)
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
}