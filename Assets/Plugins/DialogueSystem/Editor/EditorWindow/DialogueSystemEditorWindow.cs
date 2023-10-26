using DialogueSystem.Utilities;
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

        #region Elements Addition
        private void AddGraphView()
        {
            DialogueSystemGraphView grathView = new DialogueSystemGraphView(this);
            
            grathView.StretchToParentSize();

            rootVisualElement.Add(grathView);
        }
        #endregion

        #region Styles
        private void AddStyles()
        {
            rootVisualElement.LoadAndAddStyleSheets(stylesLink);
        }
        #endregion

    }
}
