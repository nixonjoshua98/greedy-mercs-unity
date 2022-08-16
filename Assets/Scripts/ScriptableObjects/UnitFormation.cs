using System.Collections.Generic;
using UnityEngine;

namespace SRC.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/UnitFormation")]
    public class UnitFormation : ScriptableObject
    {
        public List<Vector2> RelativePositions;
    }
}
