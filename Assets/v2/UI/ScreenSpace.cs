using UnityEngine;

namespace GM.UI
{
    /*
     Important. It is difficult to get the screen space width. Screen.Width, Displays..., etc. are not always accurate when it comes to mobile devices
        due to (i think) that mobiles can downscale their screen. e.g I have my S10 rendering at only 1080p but the screen space is 1665.
        We attach this class to any object, and we *hope* that it runs and grabs the canvas width (which seems to be what we want)
        
        We should find a better approach at some point.     
     */
    public class ScreenSpace : MonoBehaviour
    {
        public static float _Width;

        public static float Width
        {
            get
            {
                if (_Width == default)
                {
                    FetchWidth();

                    if (_Width == default)
                    {
                        Debug.LogWarning("ScreenSpace.Width either has not been set or has failed to be updated");
                    }
                }

                return _Width;
            }
        }

        private static void FetchWidth()
        {
            Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

            _Width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        }
    }
}