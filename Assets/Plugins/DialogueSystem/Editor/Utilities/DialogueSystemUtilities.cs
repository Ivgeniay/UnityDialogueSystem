using DialogueSystem.Groups;
using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Utilities
{
    public static class DialogueSystemUtilities
    {
        public static TextField CreateTextField (string value = null, EventCallback<ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            TextField textField = new()
            {
                value = value,
            };

            if (onChange is not null)
                textField.RegisterValueChangedCallback(onChange);
            textField.AddToClassList(styles);
            return textField;
        }
        public static TextField CreateTextArea (string value = null, EventCallback<ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            TextField textField = CreateTextField(value, onChange, styles);
            textField.multiline = true;
            return textField;
        }
        public static Foldout CreateFoldout (string title, bool collapsed = false, string[] styles = null)
        {
            var foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            foldout.AddToClassList(styles);
            return foldout;
        }
        public static Button CreateButton(string text, Action onClick = null, string[] styles = null)
        {
            Button btn = new Button(onClick)
            {
                text = text,
            };
            btn.AddToClassList(styles);
            return btn;
        }
        public static Port CreatePort(this BaseNode baseNode, string portname = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single, Color color = default, Type type = null, bool portCapLit = false)
        {
            Type _type = typeof(bool);
            if (color == default) color = Color.white;
            if (type != null) _type = type;
            Port port = baseNode.InstantiatePort(orientation, direction, capacity, _type);
            port.portName = portname;
            port.portColor = color;
            port.portCapLit = portCapLit;

            return port;
        }
        public static BaseGroup CreateGroup(DialogueSystemGraphView graphView, Type type, Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null)
        {
            var group = new BaseGroup(title, mousePosition)
            {
                tooltip = tooltip == null ? title : tooltip,
            };

            graphView.AddGroup(group);
            return group;
        }
        public static BaseNode CreateNode(DialogueSystemGraphView graphView, Type type, Vector2 position)
        {
            if (typeof(BaseNode).IsAssignableFrom(type))
            {
                BaseNode node = (BaseNode)Activator.CreateInstance(type);
                node.Initialize(graphView, position);
                node.Draw();

                graphView.AddUngroupedNode(node);
                return node;
            }
            else
                throw new ArgumentException("Type must be derived from BaseNode", nameof(type));
        }


        public static List<Type> GetListExtendedClasses(Type baseType)
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
            catch { }
            return nodeTypes;
        }
    }
}
