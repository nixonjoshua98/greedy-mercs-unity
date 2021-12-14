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
            if (!TryGetAvailablePooledObject(out GameObject obj))
            {
                obj = Instantiate(PooledObject);

                Objects.Add(obj);
            }

            obj.SetActive(true);

            return obj;
        }

        public T Spawn<T>() where T: Object => Spawn().GetComponent<T>();

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
    }
}
