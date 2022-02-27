using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GM
{
    public class AssetBundlesManager
    {
        AssetBundle IconsBundle;

        public Sprite LoadIcon(string name) => IconsBundle.LoadAsset<Sprite>(name);

        public void Load()
        {
            bool loaded = LoadAssetBundles();

            if (!loaded)
            {
                Debug.LogError("Asset bundles failed to load");
            }
            else
            {
                Debug.Log("Asset bundles loaded");
            }
        }

        bool LoadAssetBundles()
        {
            IconsBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "bundles", GM.Common.Constants.AssetBundles.Icons));

            if (IconsBundle == null) return false;


            return true;            
        }
    }
}