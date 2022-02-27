using UnitID = GM.Common.Enums.UnitID;
using GM.UI;

namespace GM.Mercs.UI
{
    public abstract class MercUIObject : SlotObject
    {
        UnitID AssignedId;

        public void Assign(UnitID id)
        {
            AssignedId = id;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        public Data.MercData AssignedMerc => App.Data.Mercs.GetMerc(AssignedId);
    }
}
