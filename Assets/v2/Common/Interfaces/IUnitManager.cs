using System.Collections.Generic;
using UnityEngine;

namespace GM.Common.Interfaces
{
    public interface IUnitManager
    {
        List<GM.Units.UnitBaseClass> EnemyUnits { get; set; }
        int NumEnemyUnits { get; }

        bool TryGetEnemyUnit(out GM.Units.UnitBaseClass unit);
        GameObject InstantiateEnemyUnit();
    }
}
