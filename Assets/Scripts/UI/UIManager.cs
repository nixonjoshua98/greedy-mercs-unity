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

        [SerializeField] private GameObject DefaultLayerObject;
        private readonly Dictionary<UILayer, GameObject> Layers = new();

        private void Awake()
        {
            Instances.Add(this);
        }

        private void OnDestroy()
        {
            Instances.Remove(this);
        }

        public T Create<T>(GameObject objectToInst, UILayer layer = UILayer.ONE)
        {
            return Create(objectToInst, layer).GetComponent<T>();
        }

        public GameObject Create(GameObject objectToInst, UILayer layer = UILayer.ONE)
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
