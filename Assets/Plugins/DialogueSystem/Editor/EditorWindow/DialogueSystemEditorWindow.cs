using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class DialogueSystemEditorWindow : EditorWindow
    {
        [MenuItem("DES/DialogueSystem")]
        public static void ShowExample()
        {
            DialogueSystemEditorWindow wnd = GetWindow<DialogueSystemEditorWindow>();
            wnd.titleContent = new GUIContent("Dialogue Graph Window");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

        }
    }
}
