using GM.Common.Enums;
using GM.Mercs.Controllers;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GM.Units
{
    class EnemyUnitTarget
    {
        public UnitBase Unit;
        public int NumRangedAttackers;
        public List<MercController> MeleeAttackers = new List<MercController>(2) { null, null };
    }

    public enum AttackSide
    {
        None,
        Left,
        Right
    }

    public class AttackTarget
    {
        public UnitBase Unit;
        public AttackSide MeleeAttackSide;
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

        public bool Contains(UnitBase unit)
        {
            return _units.Find(x => x.Unit == unit) is not null;
        }

        public bool Contains(AttackTarget unit)
        {
            return unit is not null && _units.Find(x => x.Unit == unit.Unit) is not null;
        }

        public UnitBase Last()
        {
            return _units[_units.Count - 1].Unit;
        }

        public UnitBase First()
        {
            return _units[0].Unit;
        }

        public bool GetTarget(MercController controller, out AttackTarget unit)
        {
            unit = default;

            if (_units.Count == 0 || !CanAttackTypeTargetUnit(controller.MercDataValues.AttackType, _units[0]))
                return false;

            AddTarget(controller, _units[0]);

            unit = new AttackTarget()
            {
                Unit = _units[0].Unit,
                MeleeAttackSide = GetAttackSide(controller, _units[0])
            };

            return true;
        }

        public void ReleaseTarget(MercController controller, AttackTarget unit)
        {
            var target = _units.Find(x => x.Unit == unit.Unit);

            if (target != null)
            {
                RemoveTarget(controller, target);
            }
        }

        void AddTarget(MercController attacker, EnemyUnitTarget defender)
        {
            switch (attacker.MercDataValues.AttackType)
            {
                case UnitAttackType.Ranged:
                    defender.NumRangedAttackers += 1;
                    break;

                case UnitAttackType.Melee:
                    int idx = defender.MeleeAttackers.IndexOf(null);
                    
                    defender.MeleeAttackers[idx] = attacker;

                    break;
            }
        }

        AttackSide GetAttackSide(MercController controller, EnemyUnitTarget defender)
        {
            int idx = defender.MeleeAttackers.IndexOf(controller);

            return idx switch
            {
                0 => AttackSide.Left,
                1 => AttackSide.Right,
                _ => AttackSide.None
            };
        }

        void RemoveTarget(MercController attacker, EnemyUnitTarget defender)
        {
            switch (attacker.MercDataValues.AttackType)
            {
                case UnitAttackType.Ranged:
                    defender.NumRangedAttackers -= 1;
                    break;

                case UnitAttackType.Melee:
                    int idx = defender.MeleeAttackers.IndexOf(attacker);

                    if (idx >= 0)
                        defender.MeleeAttackers[idx] = null;

                    break;
            }
        }

        bool CanAttackTypeTargetUnit(UnitAttackType type, EnemyUnitTarget target)
        {
            return type switch
            {
                UnitAttackType.Melee => target.MeleeAttackers.Where(x => x is not null).Count() < 2,
                UnitAttackType.Ranged => target.NumRangedAttackers == 0,
                _ => false
            };
        }
    }
}
