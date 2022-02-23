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

        public bool DealDamageToTarget(BigDouble damageValue)
        {
            if (UnitManager.NumEnemyUnits == 0)
                return false;

            GM.Units.UnitBaseClass unit = UnitManager.GetNextEnemyUnit();

            GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

            health.TakeDamage(damageValue);

            DamageNumberManager.Spawn(unit.Avatar, Format.Number(damageValue));

            return true;
        }
    }
}