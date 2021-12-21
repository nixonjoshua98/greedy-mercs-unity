using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Units.Formations
{
    [CreateAssetMenu(menuName = "Scriptables/UnitFormation")]
    public class UnitFormation : ScriptableObject
    {
        [SerializeField] List<Vector2> positions;

        public int NumPositions { get { return positions.Count; } }

        public List<Vector2> Positions(Vector2 centerPosition)
        {
            float xOffsetFromZero = positions.Average(pos => pos.x);

            return positions.Select(pos => {
                // Shift position so the formation is based around (0,0) being the center
                Vector2 actualPos = pos - new Vector2(xOffsetFromZero, 0);

                return actualPos + centerPosition;
            }).ToList();
        }

        public Vector2 GetPosition(int i)
        {
            return positions[i];
        }

        public Vector2 MinBounds() => positions.OrderBy(v => v.x).First();
    }
}
