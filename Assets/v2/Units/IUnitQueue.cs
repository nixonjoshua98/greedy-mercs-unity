using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    public interface IUnitQueue
    {
        int NumUnits { get; }

        bool TryGetUnit(ref UnitBaseClass current);
        bool ContainsUnit(UnitBaseClass unit);
    }


    public interface IEnemyUnitQueue : IUnitQueue
    {

    }


    public interface IFriendlyUnitQueue : IUnitQueue
    {

    }
}
