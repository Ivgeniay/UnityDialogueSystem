using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.TextFields;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Window;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;

namespace DialogueSystem.Utilities
{
    public static class DSUtilities
    {
        public static Label CreateLabel(string value = null, EventCallback<ChangeEvent<string>> onClick = null, string[] styles = null)
        {
            Label label = new Label()
            {
                text = value,
            };
            if (onClick is not null) label.RegisterCallback(onClick);
            label.AddToClassList(styles);
            return label;
        }
        public static Toggle CreateToggle(string text = null, string label = null, EventCallback<ChangeEvent<bool>> onChange = null, string[] styles = null, bool value = false)
        {
            Toggle toggle = new Toggle
            {
                value = value,
                text = text,
                label = label,
            };
            if (onChange is not null) toggle.RegisterValueChangedCallback(onChange);
            toggle.AddToClassList(styles);
            return toggle;
        }
        public static FloatField CreateFloatField(float value = 0, string label = null, EventCallback<ChangeEvent<float>> onChange = null, string[] styles = null)
        {
            FloatField floatField = new FloatField()
            {
                value = value,
                label = label
            };
            if (onChange is not null) floatField.RegisterValueChangedCallback(onChange);
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
            if (onChange is not null) integerField.RegisterValueChangedCallback(onChange);
            integerField.AddToClassList(styles);
            return integerField;
        }
        public static ProgressBar CreateProgressBar(float value = 0, float lowValue = 0, float maxValue = 1, string title = "", EventCallback<ChangeEvent<float>> onChange = null, string[] styles = null)
        {
            ProgressBar progressBar = new ProgressBar()
            {
                lowValue = lowValue,
                highValue = maxValue,
                value = value,
                title = title,
            };
            if (onChange is not null) progressBar.RegisterValueChangedCallback(onChange);
            progressBar.AddToClassList(styles);
            return progressBar;
        }
        public static TextField CreateTextField (string value = null, string label = null, EventCallback<ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            TextField textField = new()
            {
                value = value,
                label = label,
            };

            if (onChange is not null) textField.RegisterValueChangedCallback(onChange);
            textField.AddToClassList(styles);
            return textField;
        }
        public static TextField CreateTextArea(string value = null, string label = null, EventCallback < ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            TextField textField = CreateTextField(value, label, onChange, styles);
            textField.multiline = true;
            return textField;
        }

        public static DSTextField CreateDSTextField(DSGraphView graphView, string value = null, string label = null, EventCallback<ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            DSTextField textField = new()
            {
                value = value,
                label = label,
            };
            textField.Initialize(graphView);
            textField.IsSerializedInScript = true;
            if (onChange is not null) textField.RegisterValueChangedCallback(onChange);
            textField.AddToClassList(styles);
            return textField;
        }
        public static DSTextField CreateDSTextArea(DSGraphView graphView, string value = null, string label = null, EventCallback<ChangeEvent<string>> onChange = null, string[] styles = null)
        {
            DSTextField textField = CreateDSTextField(graphView, value, label, onChange, styles);
            textField.multiline = true;
            return textField;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false, string[] styles = null)
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
        public static BasePort CreatePort(this BaseNode baseNode, string ID, string portname = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single, Color color = default, Type type = null, object defaultValue = null)
        {
            if (color == default) color = Color.white;
            type = type == null ? typeof(bool) : type;
            BasePort port = baseNode.InstantiatePort(orientation, direction, capacity, type) as BasePort;

            port.portName = portname;
            port.portColor = color;
            port.SetValue(defaultValue);
            port.SetPortType(type);
            port.ID = ID;

            return port;
        }
       
        public static BaseGroup CreateGroup(DSGraphView graphView, Type type, Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null)
        {
            var group = new BaseGroup(title, mousePosition)
            {
                tooltip = tooltip == null ? title : tooltip,
            };

            graphView.AddGroup(group);
            return group;
        }
        public static BaseNode CreateNode(DSGraphView graphView, Type type, Vector2 position, List<object> portsContext)
        {
            if (typeof(BaseNode).IsAssignableFrom(type))
            {
                BaseNode node = (BaseNode)Activator.CreateInstance(type);
                node.Initialize(graphView, position, context: portsContext);
                node.OnCreate();

                graphView.AddUngroupedNode(node);
                return node;
            }
            else
                throw new ArgumentException("Type must be derived from BaseNode", nameof(type));
        }

        public static object GetDefaultValue(Type type)
        {
            if (type == null) throw new ArgumentNullException();
            object result = default(object);
            if (type.IsValueType) result = Activator.CreateInstance(type);
            else if (type == typeof(string)) result = string.Empty;
            return result;
        }


        public static List<Type> GetListExtendedClasses(Type baseType)
        {
            var nodeTypes = GetListExtendedClasses(baseType, Assembly.GetExecutingAssembly());
            try
            {
                Assembly assemblyCSharp = Assembly.Load("Assembly-CSharp-Editor");
                List<Type> derivedTypesFromCSharp = GetListExtendedClasses(baseType, assemblyCSharp);
                foreach (Type type in derivedTypesFromCSharp)
                    nodeTypes.Add(type);
            }
            catch { }
            return nodeTypes;
        }
        public static List<Type> GetListExtendedClasses(Type baseType, Assembly assembly) =>
            assembly.GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .ToList();
        public static List<Type> GetListExtendedIntefaces(Type interfaceType, Assembly assembly) =>
            assembly.GetTypes()
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToList();

        public static bool IsAvalilableType(Type type) => DSConstants.AvalilableTypes.Contains(type);
        public static bool IsPrimitiveType(Type type) => DSConstants.PrimitiveTypes.Contains(type);
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

        public static string GenerateClassNameFromType(Type t)
        {
            var name = t.Name.Replace("node", "", StringComparison.OrdinalIgnoreCase);
            name = name.Replace("base", "", StringComparison.OrdinalIgnoreCase);
            name = char.ToUpper(name[0]) + name.Substring(1);
            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]))
                {
                    name = name.Insert(i, "_");
                    i++;
                }
            }
            name = name.Insert(0, "DS");
            return name;
        }
        public static string GenerateClassPefixFromType(Type t)
        {
            switch (t)
            {
                case var _ when t == typeof(byte): return "b";
                case var _ when t == typeof(int): return "i";
                case var _ when t == typeof(long): return "l";
                case var _ when t == typeof(short): return "sh";
                case var _ when t == typeof(decimal): return "de";
                case var _ when t == typeof(double): return "d";
                case var _ when t == typeof(float): return "f";
                case var _ when t == typeof(string): return "s";
                case var _ when t == typeof(char): return "c";
                case var _ when t == typeof(bool): return "b";
                default: return t.Name.Length >= 2 ? t.Name.Substring(0, 2) : t.Name;
            }
        }
    }
}
