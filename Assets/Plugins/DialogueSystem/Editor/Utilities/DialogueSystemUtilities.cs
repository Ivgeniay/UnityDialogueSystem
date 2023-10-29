using DialogueSystem.Groups;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
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
        public static Label CreateLabel (string value = null, EventCallback<ChangeEvent<string>> onClick = null, string[] styles = null)
        {
            Label label = new Label()
            {
                text = value,
            };
            if (onClick is not null) label.RegisterCallback(onClick);
            label.AddToClassList(styles);
            return label;
        }

        public static FloatField CreateFloatField(float value = 0, string label = null, EventCallback<ChangeEvent<float>> onChange = null, string[] styles = null)
        {
            FloatField floatField = new FloatField()
            {
                value = value,
                label = label
            };
            if (onChange is not null)
                floatField.RegisterValueChangedCallback(onChange);
            floatField.AddToClassList(styles);
            return floatField;
        }
        public static IntegerField CreateIntegerField(int value = 0, string label = null, EventCallback<ChangeEvent<int>> onChange = null, string[] styles = null)
        {
            IntegerField integerField = new IntegerField()
            {
                value = value,
                label = label
            };
            if (onChange is not null)
                integerField.RegisterValueChangedCallback(onChange);
            integerField.AddToClassList(styles);
            return integerField;
        }
        public static TextField CreateTextField (string value = null, string label = null, EventCallback<ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            TextField textField = new()
            {
                value = value,
                label = label,
            };

            if (onChange is not null)
                textField.RegisterValueChangedCallback(onChange);
            textField.AddToClassList(styles);
            return textField;
        }
        public static TextField CreateTextArea (string value = null, string label = null, EventCallback < ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            TextField textField = CreateTextField(value, label, onChange, styles);
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
        public static BasePort CreatePort(this BaseNode baseNode, string portname = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single, Color color = default, Type type = null, object defaultValue = null)
        {
            if (color == default) color = Color.white;
            type = type == null ? typeof(bool) : type;
            BasePort port = baseNode.InstantiatePort(orientation, direction, capacity, type) as BasePort;

            port.portName = portname;
            port.portColor = color;
            port.Value = defaultValue;
            port.portType = type;

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
                node.OnCreate();

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
                    nodeTypes.Add(type);
                
            }
            catch { }
            return nodeTypes;
        }
        public static string GenerateWindowSearchNameFromType(Type t)
        {
            var name = t.Name.Replace("node", "", StringComparison.OrdinalIgnoreCase);
            name = name.Replace("base", "", StringComparison.OrdinalIgnoreCase);
            name = char.ToUpper(name[0]) + name.Substring(1);
            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]))
                {
                    name = name.Insert(i, " ");
                    i++;
                }
            }
            return name;
        }
    }
}
