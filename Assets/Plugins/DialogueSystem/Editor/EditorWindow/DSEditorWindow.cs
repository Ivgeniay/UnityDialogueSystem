using DialogueSystem.Toolbars;
using DialogueSystem.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueSystem.Window
{
    public class DSEditorWindow : EditorWindow
    {
        private string stylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemVariables.uss";
        DSGraphView grathView;


        [MenuItem("DES/Dialogue Graph")]
        public static void OpenWindow()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
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
            grathView = new DSGraphView(this);
            grathView.StretchToParentSize();

            rootVisualElement.Add(grathView);
        }
        private void AddToolbar()
        {
            DSToolbar toolbar = new(grathView);
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
