﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    public static class MonoBehaviourExtensions
    {
        public static void InvokeAfter(this MonoBehaviour mono, float delay, Action callback)
        {
            mono.StartCoroutine(InvokeAfter(delay, callback));
        }

        public static void Lerp01(this MonoBehaviour mono, float duration, Action<float> action)
        {
            mono.StartCoroutine(Lerp(0, 1, duration, action));
        }

        public static T GetComponentInScene<T>(this MonoBehaviour _)
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootGameObject in rootGameObjects)
            {
                T component = rootGameObject.GetComponentInChildren<T>();

                if (component is not null)
                    return component;
            }

            return default;
        }

        public static T Instantiate<T>(this MonoBehaviour mono, GameObject obj, Transform parent) => UnityEngine.Object.Instantiate(obj, parent).GetComponent<T>();
        public static GameObject InstantiateUI(this MonoBehaviour mono, GameObject obj) => UnityEngine.Object.Instantiate(obj, MainCanvas.transform);
        public static T InstantiateUI<T>(this MonoBehaviour mono, GameObject obj) => InstantiateUI(mono, obj).GetComponent<T>();

        // Private Static Methods //

        private static GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        static IEnumerator Lerp(float from, float to, float duration, Action<float> action)
        {
            float progress = 0;

            while (progress < 1)
            {
                action.Invoke(Mathf.Lerp(from, to, progress));

                progress += (Time.unscaledDeltaTime / duration);

                yield return new WaitForEndOfFrame();
            }

            action.Invoke(to);
        }

        static IEnumerator InvokeAfter(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback.Invoke();
        }
    }
}