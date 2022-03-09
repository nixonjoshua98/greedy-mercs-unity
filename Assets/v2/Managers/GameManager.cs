using GM.DamageTextPool;
using GM.Units;
using GM.Controllers;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        IEnemyUnitQueue EnemyUnits;

        WaveManager WaveManager;
        IDamageTextPool DamageNumberManager;

        void Awake()
        {
            EnemyUnits = this.GetComponentInScene<IEnemyUnitQueue>();

            WaveManager = this.GetComponentInScene<WaveManager>();
            DamageNumberManager = this.GetComponentInScene<IDamageTextPool>();
        }

        void Start()
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