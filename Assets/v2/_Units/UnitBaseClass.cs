namespace GM.Units
{
    public abstract class UnitBaseClass : GM.Core.GMMonoBehaviour
    {
        public GM.Units.UnitAvatar Avatar;

        public IMovementController Movement { get; set; }

        protected virtual void GetComponents()
        {
            Movement = GetComponent<IMovementController>();
        }
    }
}
