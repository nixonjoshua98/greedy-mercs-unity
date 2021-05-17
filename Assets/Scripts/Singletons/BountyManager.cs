using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Bounty
{
    public class BountyManager : MonoBehaviour
    {
        public static BountyManager Instance = null;

        public static BountyManager Create(JSONNode node)
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            GameObject obj = new GameObject("BountyManager [Created]");

            Instance = obj.AddComponent<BountyManager>();

            Instance.Setup(node);

            return Instance;
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Setup(JSONNode node)
        {

        }

        void Start()
        {

        }
    }
}