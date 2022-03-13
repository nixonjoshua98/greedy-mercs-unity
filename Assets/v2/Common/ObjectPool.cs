using System.Collections.Generic;
using UnityEngine;

namespace GM.Common
{
    class ObjectPoolObject
    {
        public readonly GameObject GameObject;

        float lastUsedTime;

        public ObjectPoolObject(GameObject obj)
        {
            GameObject = obj;
        }

        public void Set()
        {
            lastUsedTime = Time.time;
        }

        public bool Expired(float expireTimer = 30.0f) => (Time.time - lastUsedTime) >= expireTimer;
    }

    public class ObjectPool : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject PooledObject;
        [SerializeField] Transform ObjectParent = null;

        List<ObjectPoolObject> Objects = new List<ObjectPoolObject>();

        GameObject Spawn()
        {
            DestroyExpiredObjects();

            if (!TryGetAvailablePooledObject(out ObjectPoolObject obj))
            {
                obj = new ObjectPoolObject(Instantiate(PooledObject, ObjectParent));

                Objects.Add(obj);
            }

            GameObject go = obj.GameObject;

            go.SetActive(true);
            obj.Set();

            return go;
        }

        public T Spawn<T>() where T : Object => Spawn().GetComponent<T>();

        bool TryGetAvailablePooledObject(out ObjectPoolObject obj)
        {
            obj = null;

            foreach (ObjectPoolObject pooledObject in Objects)
            {
                if (!pooledObject.GameObject.activeInHierarchy)
                {
                    obj = pooledObject;

                    return true;
                }
            }

            return false;
        }

        void DestroyExpiredObjects()
        {
            for (int i = 0; i < Objects.Count; ++i)
            {
                var current = Objects[i];

                if (current.Expired())
                {
                    Destroy(current.GameObject);
                    Objects[i] = null;
                }
            }

            Objects.RemoveAll(x => x == null || x.GameObject == null);
        }
    }
}
