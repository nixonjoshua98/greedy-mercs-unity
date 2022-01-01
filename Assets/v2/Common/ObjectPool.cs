using System.Collections.Generic;
using UnityEngine;

namespace GM.Common
{
    public class ObjectPool : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject PooledObject;
        [SerializeField] Transform ObjectParent = null;

        List<GameObject> Objects = new List<GameObject>();

        GameObject Spawn()
        {
            if (!TryGetAvailablePooledObject(out GameObject obj))
            {
                obj = InstantiatePooledObject();

                Objects.Add(obj);
            }

            obj.SetActive(true);

            return obj;
        }

        public T Spawn<T>() where T: Object => Spawn().GetComponent<T>();
        public T Spawn<T>(Vector3 position) where T : Object
        {
            GameObject o = Spawn();
            o.transform.position = position;
            return o.GetComponent<T>();
        }

        GameObject InstantiatePooledObject()
        {
            if (ObjectParent == null)
            {
                return Instantiate(PooledObject);
            }
            return Instantiate(PooledObject, ObjectParent);
        }

        bool TryGetAvailablePooledObject(out GameObject obj)
        {
            obj = null;

            Objects.RemoveAll(x => x == null);

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
    }
}