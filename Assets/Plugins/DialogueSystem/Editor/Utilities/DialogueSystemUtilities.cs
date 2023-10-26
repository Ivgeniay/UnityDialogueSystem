using Codice.Client.BaseCommands.BranchExplorer;
using DialogueSystem.Nodes;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
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
    }
}
