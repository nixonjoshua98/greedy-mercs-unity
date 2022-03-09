using GM.DamageTextPool;
using GM.Units;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        IEnemyUnitFactory UnitManager;
        IEnemyUnitQueue EnemyUnits;

        WaveManager WaveManager;
        IDamageTextPool DamageNumberManager;

        void Awake()
        {
            EnemyUnits = this.GetComponentInScene<IEnemyUnitQueue>();

            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
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

            GM.Controllers.AbstractHealthController health = unit.GetCachedComponent<GM.Controllers.AbstractHealthController>();

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