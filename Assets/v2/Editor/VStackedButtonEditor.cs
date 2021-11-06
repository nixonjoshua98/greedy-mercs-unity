using TMPro;
using UnityEditor;
using UnityEngine;

namespace GM.UI
{
    [CustomEditor(typeof(VStackedButton))]
    public class VStackedButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {

            VStackedButton component = (VStackedButton)target;

            base.OnInspectorGUI();

            EditorGUILayout.LabelField("References", EditorStyles.boldLabel);

            component.TopText = (TMP_Text)EditorGUILayout.ObjectField("Top Text", component.TopText, typeof(TMP_Text), true);
            component.BtmText = (TMP_Text)EditorGUILayout.ObjectField("Bottom Text", component.BtmText, typeof(TMP_Text), true);
        }
    }
}
