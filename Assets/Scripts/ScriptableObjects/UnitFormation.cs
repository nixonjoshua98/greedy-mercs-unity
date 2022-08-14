using System.Collections.Generic;
using UnityEngine;

namespace GM.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/UnitFormation")]
    public class UnitFormation : ScriptableObject
    {
        public List<Vector2> RelativePositions;
    }
}
