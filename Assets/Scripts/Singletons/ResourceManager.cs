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

        public ResourceManager()
        {
            CachedSprites = new Dictionary<string, Sprite>();
        }


        public static Sprite LoadSprite(string path) { return Instance._LoadSprite(path); }


        Sprite _LoadSprite(string path)
        {
            if (CachedSprites.ContainsKey(path))
                return CachedSprites[path];

            CachedSprites[path] = Resources.Load<Sprite>(path);

            return CachedSprites[path];
        }
    }
}