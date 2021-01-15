using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class UsernameChange : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text UsernameText;

        [Header("Prefabs")]
        [SerializeField] GameObject TextInputObject;

        TextInputMessage spawnedTextInput;

        void FixedUpdate()
        {
            UsernameText.text = GameState.Player.username;
        }

        // === Callbacks ===

        public void OnClick()
        {
            bool Verify(string s)
            {
                return s.Length >= 3;
            }

            spawnedTextInput = Utils.UI.Instantiate(TextInputObject, Vector3.zero).GetComponent<UI.TextInputMessage>();

            spawnedTextInput.Init("Change Username", "Enter your new username!", Verify, OnUsernameChange);
        }

        void OnUsernameChange(string username)
        {
            JSONNode node = Utils.Json.GetDeviceNode();

            node["newUsername"] = username;

            void Callback(long code, string compressed) => ServerCallback(username, code, compressed);

            Server.ChangeUsername(this, Callback, node);
        }

        void ServerCallback(string username, long code, string compressed)
        {
            if (code == 200)
            {
                GameState.Player.username = username;

                spawnedTextInput.Destroy();
            }
        }
    }
}