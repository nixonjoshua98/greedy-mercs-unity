using GM.Units;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class UnitManager : MonoBehaviour, GM.Common.Interfaces.IUnitManager
    {
        readonly Vector3 LeftMostEnemyUnitStartPosition = new Vector3(8, GM.Common.Constants.CENTER_BATTLE_Y);

        [Header("Prefabs/Objects")]
        public GameObject EnemyUnitObject;

        public List<UnitBaseClass> EnemyUnits { get; set; } = new List<UnitBaseClass>();

        // Public properties
        public int NumEnemyUnits { get => EnemyUnits.Count; }

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
            Vector3 pos = LeftMostEnemyUnitStartPosition + new Vector3(Camera.main.MaxBounds().x, 0);

            if (EnemyUnits.Count > 0)
            {
                UnitBaseClass unit = GetLastPlacedEnemyUnit();

                pos = new Vector3(unit.Avatar.Bounds.max.x + (unit.Avatar.Bounds.size.x / 2) + 0.25f, unit.transform.position.y);
            }

            GameObject instObject = Instantiate(EnemyUnitObject, pos, Quaternion.identity);

            UnitBaseClass newEnemyUnit = instObject.GetComponent<UnitBaseClass>();

            // Set internal event listeners
            if (instObject.TryGetComponent(out GM.Controllers.HealthController health))
            {
                health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(newEnemyUnit));
            }

            EnemyUnits.Add(newEnemyUnit);

            return instObject;
        }

        // = Event Callbacks = //

        void OnEnemyZeroHealth(UnitBaseClass unit)
        {
            EnemyUnits.Remove(unit);
        }

        UnitBaseClass GetLastPlacedEnemyUnit() => EnemyUnits[EnemyUnits.Count - 1];
    }
}