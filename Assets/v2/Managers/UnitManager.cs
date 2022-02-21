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

        List<UnitBaseClass> EnemyUnits = new List<UnitBaseClass>();

        public GameObject InstantiateEnemyUnit()
        {
            Vector3 pos = LeftMostEnemyUnitStartPosition + new Vector3(Camera.main.transform.position.x, 0);

            if (EnemyUnits.Count > 0)
            {
                UnitBaseClass unit = GetLastPlacedEnemyUnit();

                pos = new Vector3(unit.Avatar.MaxBounds.x + (unit.Avatar.Size.x / 2) + 0.25f, unit.transform.position.y);
            }

            GameObject instObject = Instantiate(EnemyUnitObject, pos, Quaternion.identity);

            UnitBaseClass newEnemyUnit = instObject.GetComponent<UnitBaseClass>();

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