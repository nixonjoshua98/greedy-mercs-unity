using GM.Controllers;
using GM.DamageTextPool;
using GM.Units;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        private IEnemyUnitCollection EnemyUnits;
        private WaveManager WaveManager;
        private IDamageTextPool DamageNumberManager;

        private void Awake()
        {
            EnemyUnits = this.GetComponentInScene<IEnemyUnitCollection>();

            WaveManager = this.GetComponentInScene<WaveManager>();
            DamageNumberManager = this.GetComponentInScene<IDamageTextPool>();
        }

        private void Start()
        {
            WaveManager.Run();
        }


        public bool DealDamageToTarget(BigDouble damageValue, bool showDamageNumber = true)
        {
            GM.Units.UnitBase unit = null;

            if (!EnemyUnits.TryGetUnit(ref unit))
                return false;

            HealthController health = unit.GetComponent<HealthController>();

            if (!health.CanTakeDamage)
                return false;

            health.TakeDamage(damageValue);

            if (showDamageNumber)
            {
                DamageNumberManager.Spawn(unit.Avatar, Format.Number(damageValue));
            }

            return true;
        }
    }
}