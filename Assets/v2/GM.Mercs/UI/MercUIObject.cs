using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public abstract class MercUIObject : Core.GMMonoBehaviour
    {
        MercID AssignedId;

        public void Assign(MercID id)
        {
            AssignedId = id;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        protected Data.MercData AssignedMerc => App.Data.Mercs.GetMerc(AssignedId);
    }
}
