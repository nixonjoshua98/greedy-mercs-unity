using GM.Common.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    class EnemyUnitTarget
    {
        public UnitBase Unit;
    }


    public class EnemyUnitCollection : MonoBehaviour
    {
        readonly List<EnemyUnitTarget> _units = new();

        public int Count { get => _units.Count; }

        public void Add(UnitBase unit)
        {
            _units.Add(new() { Unit = unit });
        }

        public void Remove(UnitBase unit)
        {
            _units.RemoveAll(x => x.Unit == unit.gameObject);
        }

        public bool Contains(UnitBase unit)
        {
            return _units.Find(x => x.Unit == unit.gameObject) is not null;
        }

        public UnitBase Last()
        {
            return _units[_units.Count - 1].Unit;
        }

        public UnitBase First()
        {
            return _units[0].Unit;
        }

        public bool TryGet(ref UnitBase current)
        {
            if (!Contains(current) && _units.Count > 0)
                current = _units[0].Unit;

            return Contains(current);
        }
    }
}
