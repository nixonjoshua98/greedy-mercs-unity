using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SRC.UI
{
    public enum UILayer
    {
        ONE = 1,
        TWO = 2,
        THREE = 3
    }

    public class UIManager : MonoBehaviour
    {
        public static readonly List<UIManager> Instances = new();
        public static UIManager Instance => Instances.Last();

        [SerializeField] GameObject DefaultLayerObject;

        readonly Dictionary<UILayer, GameObject> Layers = new();

        void Awake()
        {
            Instances.Add(this);
        }

        void OnDestroy()
        {
            Instances.Remove(this);
        }

        public T Instantiate<T>(GameObject objectToInst, UILayer layer)
        {
            return Instantiate(objectToInst, layer).GetComponent<T>();
        }

        public GameObject Instantiate(GameObject objectToInst, UILayer layer)
        {
            var layerParent = GetUILayerParent(layer);

            GameObject instObject = Instantiate(objectToInst, layerParent.transform);

            instObject.transform.SetParent(layerParent.transform, false);
            instObject.transform.SetAsLastSibling();

            return instObject;
        }

        private GameObject GetUILayerParent(UILayer layer)
        {
            if (!Layers.TryGetValue(layer, out var layerParent))
            {
                Layers[layer] = layerParent = Instantiate(DefaultLayerObject);

                layerParent.name = $"{(int)layer}";

                layerParent.transform.SetParent(transform, false);
                layerParent.transform.SetSiblingIndex((int)layer);
            }
            return layerParent;
        }
    }
}
