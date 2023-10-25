using DialogueSystem.Nodes;
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Window
{
    internal class DialogueSystemGraphView : GraphView
    {
        private string stylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        public DialogueSystemGraphView()
        {
            AddGridBackground();
            AddManipulators();

            CreateNode();

            AddStyles();
        }

        private void CreateNode()
        {
            BaseNode node = new();
            node.Initialize();
            node.Draw();
            AddElement(node);
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = EditorGUIUtility.Load(stylesLink) as StyleSheet;
            styleSheets.Add(styleSheet);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
    }
}
