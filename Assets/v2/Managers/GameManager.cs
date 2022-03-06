using GM.DamageTextPool;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        IEnemyUnitFactory UnitManager;
        WaveManager WaveManager;
        IDamageTextPool DamageNumberManager;

        void Awake()
        {
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
            if (!UnitManager.TryGetEnemyUnit(out GM.Units.UnitBaseClass unit))
                return false;

            GM.Controllers.AbstractHealthController health = unit.GetComponent<GM.Controllers.AbstractHealthController>();

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