using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class StraightProjectile : MonoBehaviour
    {
        Action callback;

        public void Init(Action _callback, float distance)
        {
            callback = _callback;

            StartCoroutine(ProjectileLoop(distance));
        }

        IEnumerator ProjectileLoop(float distance)
        {
            float progress = 0.0f;

            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + new Vector3(distance, 0, 0);

            while (progress < 1.0f)
            {
                progress += (Time.deltaTime * 2);

                transform.position = Vector3.Lerp(startPos, endPos, progress);

                yield return new WaitForEndOfFrame();
            }

            OnFinished();
        }

        void OnFinished()
        {
            callback();

            Destroy(gameObject);
        }
    }
}