using UnityEngine;

namespace GM.UI
{
    public class StarsController : MonoBehaviour
    {
        public GameObject[] stars;

        public void Show(int num)
        {
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i].SetActive(i < num);
            }
        }
    }
}