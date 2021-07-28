using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units.Formations
{
    [CreateAssetMenu(menuName = "Scriptables/UnitFormation")]
    public class UnitFormation : ScriptableObject
    {
        [SerializeField] List<Vector3> positions;

        public int numPositions { get { return positions.Count; } }

        public Vector3 GetPosition(int i)
        {
            return positions[i];
        }

        public Vector3 MinBounds() => positions.OrderBy(v => v.x).First();
    }
}
