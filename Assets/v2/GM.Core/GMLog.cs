using UnityEngine;

namespace GM.Core
{
    /* Static class which provides some utility functionality for logging using the Unity logger. */
    public class GMLog
    {
        /// <summary>
        /// Log the message provided if the object is null
        /// </summary>
        public static void LogIfNull(object o, string msg)
        {
            if (o == null)
            {
                Debug.Log(msg);
            }
        }
    }
}
