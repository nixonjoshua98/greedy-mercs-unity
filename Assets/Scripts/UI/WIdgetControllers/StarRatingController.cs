using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.UI
{
    public class StarRatingController : MonoBehaviour
    {
        [SerializeField] GameObject[] stars;

        public void UpdateRating(int rating)
        {
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i].SetActive(i < rating);
            }
        }
    }
}