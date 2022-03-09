namespace GM.Mercs.Controllers
{
    public interface IUnitActionController
    {
        int Priority { get; }
        bool HasControl { get; }

        bool WantsControl();
        void GiveControl();
        void RemoveControl();
    }
}
