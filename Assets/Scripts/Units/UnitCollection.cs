using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    public class UnitCollection : MonoBehaviour
    {
        List<UnitBase> _units = new List<UnitBase>();

        public int Count { get => _units.Count; }

        public void Add(UnitBase unit)
        {
            _units.Add(unit);
        }

        public void Remove(UnitBase unit)
        {
            _units.Remove(unit);
        }

        public bool ContainsUnit(UnitBase unit)
        {
            return _units.Contains(unit);
        }

        public UnitBase Last() => _units[_units.Count - 1];
        public UnitBase First() => _units[0];

        public bool TryGetUnit(ref UnitBase current)
        {
            if (!ContainsUnit(current) && _units.Count > 0)
                current = _units[0];

            return ContainsUnit(current);
        }
    }
}
