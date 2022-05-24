namespace GM.Units
{
    public interface IEnemyUnitCollection
    {
        bool TryGetUnit(ref UnitBase current);
        bool ContainsUnit(UnitBase unit);
    }
}
