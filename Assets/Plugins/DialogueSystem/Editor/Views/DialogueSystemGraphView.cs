using DialogueSystem.Dialogue;
using DialogueSystem.Nodes;
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
        private string stylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        public DialogueSystemGraphView()
        {
            AddGridBackground();
            AddManipulators();

            AddStyles();
        }

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

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var listNodeTypes = GetListExtendedClasses(typeof(BaseNode));
            foreach (var item in listNodeTypes)
                this.AddManipulator(CreateNodeContextMenu($"Add {item.Name}", item));
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
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

        private List<Type> GetListExtendedClasses(Type baseType)
        {
            var nodeTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .ToList();

            try
            {
                Assembly assemblyCSharp = Assembly.Load("Assembly-CSharp");
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
