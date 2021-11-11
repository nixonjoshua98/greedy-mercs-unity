using UnityEngine;

namespace GM.Targets
{
    [System.Serializable]
    public struct AttackerTarget
    {
        public Vector2? Position;

        public int AttackID;
        public GameObject Object;
    }
}
