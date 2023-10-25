using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Window
{
    public class DialogueSystemEditorWindow : EditorWindow
    {
        private string stylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemVariables.uss";

        [MenuItem("DES/Dialogue Graph")]
        public static void OpenWindow()
        {
            GetWindow<DialogueSystemEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }

        private void AddGraphView()
        {
            DialogueSystemGraphView grathView = new DialogueSystemGraphView();
            
            grathView.StretchToParentSize();

            rootVisualElement.Add(grathView);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = EditorGUIUtility.Load(stylesLink) as StyleSheet;
            rootVisualElement.styleSheets.Add(styleSheet);
        }

    }
}
