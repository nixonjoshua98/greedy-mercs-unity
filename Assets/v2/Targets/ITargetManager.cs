namespace GM.Targets
{
    public interface ITargetManager
    {
        bool TryGetMercTarget(ref GM.Units.UnitBaseClass target);
    }
}