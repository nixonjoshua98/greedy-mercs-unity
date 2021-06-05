using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Characters
{
    public class EnemyController : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<Health>().OnDeath.AddListener(OnZeroHealth);
        }

        public void OnZeroHealth(GameObject obj)
        {
            tag = "Dead";

            Destroy(gameObject, 0.1f);
        }
    }
}