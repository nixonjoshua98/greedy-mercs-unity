using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units.Formations
{
    [CreateAssetMenu(menuName = "Scriptables/UnitFormation")]
    public class UnitFormation : ScriptableObject
    {
        [SerializeField] List<Vector2> positions;

        public int NumPositions { get { return positions.Count; } }

        public Vector2 GetPosition(int i)
        {
            return positions[i];
        }

        public Vector2 MinBounds() => positions.OrderBy(v => v.x).First();
    }
}
