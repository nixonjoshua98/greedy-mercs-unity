using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextInputMessage : Message
    {
        [SerializeField] DestroyWhenClicked DestroyScript;

        [SerializeField] InputField InputField;

        Func<string, bool> Verify;
        Action<string> OnSuccess;

        // -
        string inputString = "";

        public void Init(string t, string d, Func<string, bool> verifyFunction, Action<string> successCallback)
        {
            base.Init(t, d);

            OnSuccess   = successCallback;
            Verify      = verifyFunction;
        }

        public void Destroy()
        {
            DestroyScript.DestroyNow();
        }

        public void OnConfirm()
        {
            if (Verify(inputString))
            {
                OnSuccess(inputString);
            }
        }

        public void OnInputChange()
        {
            inputString = InputField.text;
        }
    }
}
