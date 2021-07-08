
using System.Collections.Generic;

using UnityEngine;

namespace GM.Formations
{
    [CreateAssetMenu(menuName = "Scriptables/StandardUnitFormation")]
    public class StandardUnitFormation : ScriptableObject
    {
        [SerializeField] List<Vector3> positions;

        public int numPositions { get { return positions.Count; } }

        public Vector3 GetPosition(int i)
        {
            return positions[i];
        }
    }
}
