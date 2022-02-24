using GM.Units;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public interface IUnitManager
    {
        int NumEnemyUnits { get; }

        GM.Units.UnitBaseClass GetNextEnemyUnit();

        bool TryGetEnemyUnit(out GM.Units.UnitBaseClass unit);
        GameObject InstantiateEnemyUnit();
    }

    public class UnitManager : MonoBehaviour, IUnitManager
    {
        readonly Vector3 LeftMostEnemyUnitStartPosition = new Vector3(8, GM.Common.Constants.CENTER_BATTLE_Y);

        [Header("Prefabs/Objects")]
        public GameObject EnemyUnitObject;

        List<UnitBaseClass> EnemyUnits = new List<UnitBaseClass>();

        // Public properties
        public int NumEnemyUnits { get => EnemyUnits.Count; }

        public GM.Units.UnitBaseClass GetNextEnemyUnit() => EnemyUnits[0];

        public bool TryGetEnemyUnit(out GM.Units.UnitBaseClass unit)
        {
            unit = default;

            if (EnemyUnits.Count > 0)
            {
                unit = EnemyUnits[0];
                return true;
            }

            return false;
        }

        public GameObject InstantiateEnemyUnit()
        {
            GameObject obj = Instantiate(EnemyUnitObject, EnemyUnitSpawnPosition(), Quaternion.identity);

            // Components
            UnitBaseClass unit = obj.GetComponent<UnitBaseClass>();
            GM.Controllers.HealthController health = obj.GetComponent<GM.Controllers.HealthController>();

            health.Invincible = true;

            // Events
            health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(unit));

            EnemyUnits.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(unit.Avatar.Bounds.min), () => health.Invincible = false);

            return obj;
        }

        Vector3 EnemyUnitSpawnPosition()
        {
            Vector3 pos = LeftMostEnemyUnitStartPosition + new Vector3(Camera.main.MaxBounds().x, 0);

            if (EnemyUnits.Count > 0)
            {
                UnitBaseClass unit = GetLastPlacedEnemyUnit();

                pos = new Vector3(unit.Avatar.Bounds.max.x + (unit.Avatar.Bounds.size.x / 2) + 0.25f, unit.transform.position.y);
            }

            return pos;
        }

        // = Event Callbacks = //

        void OnEnemyZeroHealth(UnitBaseClass unit)
        {
            EnemyUnits.Remove(unit);
        }

        // ...

        UnitBaseClass GetLastPlacedEnemyUnit() => EnemyUnits[EnemyUnits.Count - 1];
    }
}