using DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace DialogueSystem.Window
{
    internal class DialogueSystemGraphView : GraphView
    {
        private string graphStylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        private string nodeStylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemNodeStyles.uss";
        private DialogueSearchWindow searchWindow;
        private DialogueSystemEditorWindow editorWindow;

        public DialogueSystemGraphView(DialogueSystemEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            AddGridBackground();
            AddSearchWindow();
            AddManipulators();

            AddStyles();
        }


        #region Overrides
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                    return;
                if (startPort.node == port.node)
                    return;
                if (startPort.direction == port.direction)
                    return;
                
                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var listNodeTypes = DialogueSystemUtilities.GetListExtendedClasses(typeof(BaseNode));
            foreach (var item in listNodeTypes) this.AddManipulator(CreateNodeContextMenu($"Add {item.Name}", item));
            this.AddManipulator(CreateGroupContextualMenu());
        }
        #endregion

        #region ContextMenu
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction("Add Group", a =>
                    AddElement(CreateGroup(GetLocalMousePosition(a.eventInfo.mousePosition))));

            });

            return contextualMenuManipulator;
        }


        private IManipulator CreateNodeContextMenu(string actionTitle, Type type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction(actionTitle, a =>
                    AddElement(CreateNode(type, GetLocalMousePosition(a.eventInfo.mousePosition))));
                
            });
            return contextualMenuManipulator;
        }

        private void AddSearchWindow()
        {
            if (!searchWindow)
            {
                searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();
                searchWindow.Initialize(this);
            }
            nodeCreationRequest = e => SearchWindow.Open(new SearchWindowContext(e.screenMousePosition), searchWindow);
        }
        #endregion

        #region Entities
        internal BaseNode CreateNode(Type type, Vector2 position)
        {
            if (typeof(BaseNode).IsAssignableFrom(type))
            {
                BaseNode node = (BaseNode)Activator.CreateInstance(type);
                node.Initialize(position);
                node.Draw();

                return node;
            }
            else
                throw new ArgumentException("Type must be derived from BaseNode", nameof(type));
        }

        internal Group CreateGroup(Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null)
        {
            Group group = new Group()
            {
                title = title,
                tooltip = tooltip == null ? title : tooltip,
            };
            group.SetPosition(new Rect(mousePosition, Vector2.zero));
            
            return group;
        }
        #endregion

        #region Styles
        private void AddStyles()
        {
            this.LoadAndAddStyleSheets(graphStylesLink, nodeStylesLink);
        }
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        #endregion
        #region Utilities
        internal Vector2 GetLocalMousePosition(Vector2 mousePosition) =>
            contentViewContainer.WorldToLocal(mousePosition);
        
        #endregion
    }
}
