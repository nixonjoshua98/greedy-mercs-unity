
using UnityEngine;

namespace GM.UI
{
    public class StarRatingController : MonoBehaviour
    {
        [SerializeField] GameObject[] stars;

        public void Show(int rating)
        {
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i].SetActive(i < rating);
            }
        }
    }
}