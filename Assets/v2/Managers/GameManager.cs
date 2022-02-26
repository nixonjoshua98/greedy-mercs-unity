namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        IEnemyUnitFactory UnitManager;
        WaveManager WaveManager;
        GM.UI.IDamageNumberManager DamageNumberManager;

        void Awake()
        {
            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
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
            GM.Controllers.AbstractHealthController health = unit.GetComponent<GM.Controllers.AbstractHealthController>();

            if (!health.CanTakeDamage)
                return false;

            health.TakeDamage(value);

            return true;
        }
    }
}