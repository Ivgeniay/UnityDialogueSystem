using DialogueSystem.Toolbars;
using DialogueSystem.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueSystem.Window
{
    public class DialogueSystemEditorWindow : EditorWindow
    {
        private string stylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemVariables.uss";
        DialogueSystemGraphView grathView;


        [MenuItem("DES/Dialogue Graph")]
        public static void OpenWindow()
        {
            GetWindow<DialogueSystemEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }


        #region Elements Addition
        private void AddGraphView()
        {
            grathView = new DialogueSystemGraphView(this);
            grathView.StretchToParentSize();

            rootVisualElement.Add(grathView);
        }
        private void AddToolbar()
        {
            DialogueSystemToolbar toolbar = new(grathView);
            toolbar.Initialize("DialogueFileName", "Filename: ");
            rootVisualElement.Add(toolbar);
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
