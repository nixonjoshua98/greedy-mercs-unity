using System.IO;
using UnityEditor;
using UnityEngine;

namespace GM
{
    public class BuildAssetBundles : MonoBehaviour
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void Build()
        {
            BuildPipeline.BuildAssetBundles(Path.Combine(Application.streamingAssetsPath, "bundles"), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }
    }
}
