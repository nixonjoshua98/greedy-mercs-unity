
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.UI
{
    using GM.Events;

    public class ToggleController : MonoBehaviour
    {
        [HideInInspector] protected IntegerEvent OnValueChanged;

        int currentIndex;

        void Awake()
        {
            OnValueChanged = new IntegerEvent();

            Toggle[] toggles = GetComponentsInChildren<Toggle>();

            for (int i = 0; i < toggles.Length; ++i)
            {
                Toggle t = toggles[i];  // Required otherwise the value will change in the Lambda
                int temp = i;           // ...

                t.onValueChanged.AddListener((val) => { OnToggleUpdate(temp, val); });
            }
        }

        public void AddListener(UnityAction<int> call, bool invokeEvent = true)
        {
            OnValueChanged.AddListener(call);

            if (invokeEvent)
                InvokeEvent(currentIndex);
        }

        void OnToggleUpdate(int index, bool val)
        {
            if (val)
            {
                currentIndex = index;

                InvokeEvent(currentIndex);
            }
        }

        protected virtual void InvokeEvent(int index)
        {
            OnValueChanged.Invoke(index);
        }
    }
}