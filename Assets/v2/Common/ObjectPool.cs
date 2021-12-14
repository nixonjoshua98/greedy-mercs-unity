using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GM.Common
{
    public class ObjectPool : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject PooledObject;

        List<GameObject> Objects = new List<GameObject>();

        public GameObject Spawn()
        {
            RemoveObjectsFromPool();

            if (!TryGetAvailablePooledObject(out GameObject obj))
            {
                obj = Instantiate(PooledObject);

                Objects.Add(obj);
            }

            obj.SetActive(true);

            return obj;
        }

        public T Spawn<T>(Vector3 pos) where T: Object
        {
            GameObject o = Spawn();

            o.transform.position = pos;

            return o.GetComponent<T>();
        }

        bool TryGetAvailablePooledObject(out GameObject obj)
        {
            obj = null;

            foreach (GameObject o in Objects)
            {
                if (!o.activeInHierarchy)
                {
                    obj = o;
                    return true;
                }
            }

            return false;
        }

        void RemoveObjectsFromPool()
        {
            Objects.RemoveAll(x => x == null);
        }
    }
}
