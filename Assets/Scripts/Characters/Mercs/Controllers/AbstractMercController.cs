

using UnityEngine;


namespace GM.Units
{
    public abstract class AbstractMercController : ExtendedMonoBehaviour
    {
        protected abstract bool CanAttack(GameObject obj);

        protected abstract void StartAttack(GameObject obj);

        // = = = Events = = = //
        public abstract void OnAttackHit(GameObject obj);
    }
}