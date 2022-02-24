namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        IUnitManager UnitManager;
        WaveManager WaveManager;
        GM.UI.IDamageNumberManager DamageNumberManager;

        void Awake()
        {
            UnitManager = this.GetComponentInScene<IUnitManager>();
            WaveManager = this.GetComponentInScene<WaveManager>();
            DamageNumberManager = this.GetComponentInScene<GM.UI.IDamageNumberManager>();
        }

        void Start()
        {
            WaveManager.Run();
        }

        public bool DealDamageToTarget(BigDouble damageValue, bool showDamageNumber = true)
        {
            if (!UnitManager.TryGetEnemyUnit(out GM.Units.UnitBaseClass unit))
                return false;

            bool dealt = _DealDamageToTarget(unit, damageValue);

            if (dealt && showDamageNumber)
            {
                DamageNumberManager.Spawn(unit.Avatar, Format.Number(damageValue));
            }

            return dealt;
        }

        bool _DealDamageToTarget(GM.Units.UnitBaseClass unit, BigDouble value)
        {
            GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

            if (!health.CanTakeDamage)
                return false;

            health.TakeDamage(value);

            return true;
        }
    }
}