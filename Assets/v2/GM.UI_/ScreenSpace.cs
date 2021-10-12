using UnityEngine;

namespace GM.UI_
{
    /*
     Important. It is difficult to get the screen space width. Screen.Width, Displays..., etc. are not always accurate when it comes to mobile devices
        due to (i think) that mobiles can downscale their screen. e.g I have my S10 rendering at only 1080p but the screen space is 1665.
        We attach this class to any object, and we *hope* that it runs and grabs the canvas width (which seems tobe what we want)
        
        We should find a better approach at some point.
     
     */
    public class ScreenSpace : MonoBehaviour
    {
        public static float Width;

        public void Start()
        {
            Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

            Width = canvas.GetComponent<RectTransform>().sizeDelta.x;

            Destroy(gameObject);
        }
    }
}
