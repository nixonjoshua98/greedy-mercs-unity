using GM.Units;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public interface IEnemyUnitFactory
    {
        int NumEnemyUnits { get; }

        bool TryGetEnemyUnit(out GM.Units.UnitBaseClass unit);

        GameObject InstantiateEnemyUnit();
        GameObject InstantiateEnemyBossUnit();
    }


    public class EnemyUnitFactory : MonoBehaviour, IEnemyUnitFactory
    {
        // Constants
        readonly Vector3 LeftMostEnemyUnitStartPosition = new Vector3(8, GM.Common.Constants.CENTER_BATTLE_Y);

        [Header("Prefabs/Objects")]
        public GameObject EnemyUnitObject;
        public List<GameObject> EnemyBossUnitObjects;

        List<UnitBaseClass> EnemyUnits = new List<UnitBaseClass>();

        /// <summary>
        /// Public property returning how many units are currently instantiated
        /// </summary>
        public int NumEnemyUnits { get => EnemyUnits.Count; }

        /// <summary>
        /// Preferred way of getting the first available unit
        /// </summary>
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

        /// <summary>
        /// Instantiate a regular enemy unit with minimal setup
        /// </summary>
        /// <returns></returns>
        public GameObject InstantiateEnemyUnit()
        {
            GameObject obj = Instantiate(EnemyUnitObject, EnemyUnitSpawnPosition(), Quaternion.identity);

            // Components
            UnitBaseClass unit = obj.GetComponent<UnitBaseClass>();
            GM.Controllers.AbstractHealthController health = obj.GetComponent<GM.Controllers.AbstractHealthController>();

            health.Invincible = true;

            // Events
            health.E_OnZeroHealth.AddListener(() => UnitHealthController_OnZeroHealth(unit));

            EnemyUnits.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(unit.Avatar.Bounds.min), () => health.Invincible = false);

            return obj;
        }

        /// <summary>
        /// Instantiate a enemy unit boss variant
        /// </summary>
        public GameObject InstantiateEnemyBossUnit()
        {
            GameObject unitToSpawn = EnemyBossUnitObjects[Random.Range(0, EnemyBossUnitObjects.Count - 1)];

            GameObject obj = Instantiate(unitToSpawn, EnemyUnitSpawnPosition(), Quaternion.identity);

            // Components
            UnitBaseClass unit = obj.GetComponent<UnitBaseClass>();
            GM.Controllers.AbstractHealthController health = obj.GetComponent<GM.Controllers.AbstractHealthController>();

            health.Invincible = true;

            // Events
            health.E_OnZeroHealth.AddListener(() => UnitHealthController_OnZeroHealth(unit));

            EnemyUnits.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(unit.Avatar.Bounds.min), () => health.Invincible = false);

            return obj;
        }

        /// <summary>
        /// Calculate the next unit spawn position
        /// </summary>
        Vector3 EnemyUnitSpawnPosition()
        {
            Vector3 pos = LeftMostEnemyUnitStartPosition + new Vector3(Camera.main.MaxBounds().x, 0);

            if (EnemyUnits.Count > 0)
            {
                UnitBaseClass unit = EnemyUnits[EnemyUnits.Count - 1];

                pos = new Vector3(unit.Avatar.Bounds.max.x + (unit.Avatar.Bounds.size.x / 2) + 0.25f, unit.transform.position.y);
            }

            return pos;
        }

        /// <summary>
        /// 'HealthController' callback for when the unit is defeated (health hits zero)
        /// </summary>
        /// <param name="unit"></param>
        void UnitHealthController_OnZeroHealth(UnitBaseClass unit)
        {
            EnemyUnits.Remove(unit);
        }
    }
}