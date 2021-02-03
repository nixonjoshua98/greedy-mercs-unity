using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GreedyMercs.UI.Messages
{
    public class YesNoPrompt : Message
    {
        UnityAction confirmCallback;

        public void Init(string t, string d, UnityAction _confirmCallback)
        {
            base.Init(t, d);

            confirmCallback = _confirmCallback;
        }

        public void OnClick(int index)
        {
            if (index == 1)
            {
                confirmCallback();
            }

            Destroy(gameObject);
        }
    }
}