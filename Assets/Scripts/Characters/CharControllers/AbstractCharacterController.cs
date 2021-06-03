

using UnityEngine;


namespace GM.Characters
{
    public abstract class AbstractCharacterController : ExtendedMonoBehaviour
    {
        protected abstract bool CanAttack(GameObject obj);

        protected abstract void StartAttack(GameObject obj);

        // = = = Events = = = //
        public abstract void OnAttack();
    }
}