using UnityEngine;

namespace GM
{
    public class CanvasUtils
    {
        public static GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        // = = = Instantiate = = = //
        public static GameObject Instantiate(GameObject o) => Instantiate(o, MainCanvas);

        public static T Instantiate<T>(GameObject o) where T: Component
        {
            GameObject inst = Instantiate(o, MainCanvas);

            return inst.GetComponent<T>();            
        }

        public static GameObject Instantiate(GameObject o, GameObject parent)
        {
            GameObject inst = GameObject.Instantiate(o);

            inst.transform.SetParent(parent.transform, false);

            return inst;
        }

        public static void ShowInfo(string title, string desc)
        {
            GameObject o = Resources.Load<GameObject>("Popups/Info");

            Message msg = Instantiate(o).GetComponent<Message>();

            msg.Init(title.ToUpper(), desc);
        }
    }
}
