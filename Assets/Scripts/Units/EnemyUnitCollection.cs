using GM.Mercs.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    class EnemyUnitTarget
    {
        public UnitBase Unit;
        public bool HasAttacker;
    }

    public class AttackTarget
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

        public void Remove(GameObject obj)
        {
            _units.RemoveAll(x => x.Unit.gameObject == obj);
        }

        public bool Contains(AttackTarget unit)
        {
            return unit is not null && _units.Find(x => x.Unit == unit.Unit) is not null;
        }

        public UnitBase Last()
        {
            return _units[^1].Unit;
        }

        public UnitBase First()
        {
            return _units[0].Unit;
        }

        public bool GetTarget(AbstractMercController controller, out AttackTarget unit)
        {
            unit = default;

            if (_units.Count == 0 || _units[0].HasAttacker)
                return false;

            AddTarget(controller, _units[0]);

            unit = new AttackTarget()
            {
                Unit = _units[0].Unit
            };

            return true;
        }

        public void ReleaseTarget(AbstractMercController controller, AttackTarget unit)
        {
            var target = _units.Find(x => x.Unit == unit.Unit);

            if (target != null)
            {
                RemoveTarget(controller, target);
            }
        }

        void AddTarget(AbstractMercController attacker, EnemyUnitTarget defender)
        {
            defender.HasAttacker = true;
        }

        void RemoveTarget(AbstractMercController attacker, EnemyUnitTarget defender)
        {
            defender.HasAttacker = false;
        }
    }
}
