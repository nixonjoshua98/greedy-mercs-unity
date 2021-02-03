using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class StraightProjectile : MonoBehaviour
    {
        Action callback;

        public void Init(Action _callback, float distance, float speed)
        {
            callback = _callback;

            StartCoroutine(ProjectileLoop(distance, speed));
        }

        IEnumerator ProjectileLoop(float distance, float speed)
        {
            float progress = 0.0f;

            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + new Vector3(distance, 0, 0);

            while (progress < 1.0f)
            {
                progress += (Time.deltaTime * speed);

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