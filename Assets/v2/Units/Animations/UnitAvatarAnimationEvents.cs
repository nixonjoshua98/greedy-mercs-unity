using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class UnitAvatarAnimationEvents : MonoBehaviour
    {
        [HideInInspector] public UnityEvent Attack = new UnityEvent();
        [HideInInspector] public UnityEvent Defeat = new UnityEvent();
        [HideInInspector] public UnityEvent Hurt = new UnityEvent();

        public void InvokeAttackEvent() => Attack.Invoke();
        public void InvokeDefeatEvent() => Defeat.Invoke();
        public void InvokeHurtEvent() => Hurt.Invoke();
    }
}
