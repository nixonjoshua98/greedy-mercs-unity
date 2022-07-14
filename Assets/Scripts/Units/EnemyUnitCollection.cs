using GM.Mercs.Controllers;
using System.Collections.Generic;
using UnityEngine;
using GM.Enums;
using System.Linq;

namespace GM.Units
{
    public class AttackTarget
    {
        public UnitBase Unit;

        public Dictionary<AttackSide, AbstractMercController> MeleeAttackers = new()
        {
            { AttackSide.Left, null },
            { AttackSide.Right, null },
        };

        public AttackSide GetAttackSide(AbstractMercController controller) => MeleeAttackers.GetKeyFromValue(controller);
        public bool TryGetMeleeAttacker(AttackSide side, out AbstractMercController result) => (result = MeleeAttackers[side]) != null;
    }

    public enum AttackSide
    {
        Left, 
        Right
    }

    public class EnemyUnitCollection : MonoBehaviour
    {
        readonly List<AttackTarget> _units = new();

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

            if (_units.Count == 0 || !UnitHasAvailableTargetSlot(_units[0], controller.DataValues.AttackType))
                return false;

            AddTarget(controller, _units[0]);

            unit = _units[0];

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

        bool UnitHasAvailableTargetSlot(AttackTarget target, UnitAttackType attackType)
        {
            if (attackType == UnitAttackType.Melee)
                return target.MeleeAttackers.Where(x => x.Value is null).Count() >= 1;

            return false;
        }

        void AddTarget(AbstractMercController attacker, AttackTarget defender)
        {
            if (attacker.DataValues.AttackType == UnitAttackType.Melee)
            {
                defender.MeleeAttackers[defender.GetAttackSide(null)] = attacker;
            }
        }

        void RemoveTarget(AbstractMercController attacker, AttackTarget defender)
        {
            if (attacker.DataValues.AttackType == UnitAttackType.Melee)
            {
                defender.MeleeAttackers[defender.GetAttackSide(attacker)] = null;
            }
        }
    }
}
