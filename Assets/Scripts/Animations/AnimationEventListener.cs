using System.Reflection;
using UnityEngine;

namespace SRC.Animations
{
    public sealed class AnimationEventListener : MonoBehaviour
    {
        [Header("Debug Options")]
        [SerializeField] private bool LogEvents = true;
        [Space]
        [SerializeField] private MonoBehaviour _listener;

        public void InvokeAnimationEvent(string name)
        {
            bool invoked = InvokeEventOnListener(_listener, name);

            CoreLogger.WarnWhenTrue(!invoked, $"Event '{name}' had no listener on '{_listener.name}'");
        }

        private bool InvokeEventOnListener(MonoBehaviour listener, string name)
        {
            string methodName = $"Animation_On{name}";

            if (!TryGetMethod(listener, methodName, out MethodInfo method))
            {
                return false;
            }

            method.Invoke(listener, null);

            CoreLogger.LogWhenTrue(LogEvents, $"{listener.GetType().Name}.{method.Name} invoked on '{listener.name}'");

            return true;
        }

        private bool TryGetMethod(MonoBehaviour receiver, string methodName, out MethodInfo method)
        {
            method = receiver.GetType().GetMethod(methodName);

            return method is not null;
        }
    }
}
