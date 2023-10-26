using DialogueSystem.Nodes;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Window
{
    internal class DialogueSystemGraphView : GraphView
    {
        private string graphStylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        private string nodeStylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemNodeStyles.uss";
        public DialogueSystemGraphView()
        {
            AddGridBackground();
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

            var listNodeTypes = GetListExtendedClasses(typeof(BaseNode));
            foreach (var item in listNodeTypes)
                this.AddManipulator(CreateNodeContextMenu($"Add {item.Name}", item));

            this.AddManipulator(CreateGroupContextualMenu());
        }
        #endregion

        #region ContextMenu
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction("Add Group", a =>
                    AddElement(CreateGroup("DialogueGroup", a.eventInfo.mousePosition)));

            });

            return contextualMenuManipulator;
        }


        private IManipulator CreateNodeContextMenu(string actionTitle, Type type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction(actionTitle, a =>
                    AddElement(CreateNode(type, a.eventInfo.mousePosition)));
                
            });

            return contextualMenuManipulator;
        }
        #endregion

        #region Entities
        private BaseNode CreateNode(Type type, Vector2 position)
        {
            if (typeof(BaseNode).IsAssignableFrom(type))
            {
                BaseNode node = (BaseNode)Activator.CreateInstance(type);
                node.Initialize(position);
                node.Draw();

                return node;
            }
            else
            {
                throw new ArgumentException("Type must be derived from BaseNode", nameof(type));
            }
        }
        private Group CreateGroup(string title, Vector2 mousePosition)
        {
            Group group = new Group()
            {
                title = title,

            };
            group.SetPosition(new Rect(mousePosition, Vector2.zero));
            
            
            return group;
        }
        #endregion

        #region Utilits
        private List<Type> GetListExtendedClasses(Type baseType)
        {
            var nodeTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .ToList();

            try
            {
                Assembly assemblyCSharp = Assembly.Load("Assembly-CSharp-Editor");
                List<Type> derivedTypesFromCSharp = assemblyCSharp.GetTypes()
                    .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                    .ToList();

                foreach (Type type in derivedTypesFromCSharp)
                {
                    nodeTypes.Add(type);
                }
            }
            catch {}
            return nodeTypes;
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
    }
}
