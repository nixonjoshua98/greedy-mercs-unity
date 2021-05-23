using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public class ResourceManager
    {
        static ResourceManager _Instance = null;

        static ResourceManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ResourceManager();

                return _Instance;
            }
        }

        Dictionary<string, Sprite> CachedSprites;
        Dictionary<string, GameObject> CachedPrefabs;

        public ResourceManager()
        {
            CachedSprites = new Dictionary<string, Sprite>();
            CachedPrefabs = new Dictionary<string, GameObject>();
        }


        public static Sprite LoadSprite(string path, string name) { return Instance._LoadSprite(CreatePath(path, name)); }
        public static Sprite LoadSprite(string path) { return Instance._LoadSprite(path); }
        public static GameObject LoadPrefab(string path, string name) { return Instance._LoadPrefab(CreatePath(path, name)); }
        

        // = = = Private Methods = = = 
        static string CreatePath(string path, string name)
        {
            return string.Format("{0}/{1}", path, name);
        }

        Sprite _LoadSprite(string path)
        {
            if (CachedSprites.ContainsKey(path))
                return CachedSprites[path];

            CachedSprites[path] = Resources.Load<Sprite>(path);

            return CachedSprites[path];
        }

        GameObject _LoadPrefab(string path)
        {
            if (CachedPrefabs.ContainsKey(path))
                return CachedPrefabs[path];

            CachedPrefabs[path] = Resources.Load<GameObject>(path);

            return CachedPrefabs[path];
        }
    }
}