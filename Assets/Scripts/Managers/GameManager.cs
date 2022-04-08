using GM.Controllers;
using GM.DamageTextPool;
using GM.Units;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        private IEnemyUnitQueue EnemyUnits;
        private WaveManager WaveManager;
        private IDamageTextPool DamageNumberManager;

        private void Awake()
        {
            EnemyUnits = this.GetComponentInScene<IEnemyUnitQueue>();

            WaveManager = this.GetComponentInScene<WaveManager>();
            DamageNumberManager = this.GetComponentInScene<IDamageTextPool>();
        }

        private void Start()
        {
            WaveManager.Run();
        }


        public bool DealDamageToTarget(BigDouble damageValue, bool showDamageNumber = true)
        {
            GM.Units.UnitBaseClass unit = null;

            if (!EnemyUnits.TryGetUnit(ref unit))
                return false;

            HealthController health = unit.GetCachedComponent<HealthController>();

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