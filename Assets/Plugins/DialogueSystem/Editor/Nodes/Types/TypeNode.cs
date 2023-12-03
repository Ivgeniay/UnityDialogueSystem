using DialogueSystem.Database.Save;
using System.Collections.Generic; 
using DialogueSystem.Utilities;
using DialogueSystem.UIElement;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using System.Reflection;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Nodes
{
    internal class TypeNode : BaseTypeNode
    {
        protected DSDropdownField dropdownTypes;

        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            base.Initialize(graphView, position, context);
            if (context == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.TypeTypes, Ports.PortSide.Output)
                {
                    Type = typeof(Type),
                    PortText = "Type",
                });
            }
        }

        protected override void DrawExtensionContainer(VisualElement container)
        {
            dropdownTypes = new();
            dropdownTypes.label = "Types:";

            Assembly assembly = Assembly.Load(DSConstants.DEFAULT_ASSEMBLY);
            Type[] publicTypes = assembly.GetExportedTypes();

            BasePort output = GetOutputPorts().First();

            foreach (Type type in publicTypes)
                dropdownTypes.choices.Add(type.FullName);

            if (dropdownTypes.choices.Count > 0) 
                dropdownTypes.value = output.Type == typeof(Type) ? dropdownTypes.choices[0] : output.Type.FullName;

            if (output.Type != typeof(Type))
            {
                Type type = DSUtilities.GetType(output.Type.FullName);
                if (type != null)
                {
                    dropdownTypes.value = type.FullName;
                    ChangeOutputPortsTypeAndName(type, type.Name);
                }
            }
            else
            {
                Type type = DSUtilities.GetType(dropdownTypes.value);
                if (type != null)
                {
                    dropdownTypes.value = type.FullName;
                    ChangeOutputPortsTypeAndName(type, type.Name);
                }
            }

            dropdownTypes.RegisterValueChangedCallback<string>((e) =>
            {
                Type type = DSUtilities.GetType(e.newValue);
                
                this.ChangeOutputPortsTypeAndName(type);
                output.ChangeName(type.Name);
                if (output.connected)
                {
                    List<Edge> connections = output.connections.ToList();
                    foreach (Edge edge in connections)
                    {
                        BaseNode inputNode = edge.input.node as BaseNode;
                        if (inputNode != null)
                            inputNode.OnConnectInputPort(edge.input as BasePort, edge);
                    }
                }
            });

            container.Add(dropdownTypes);
            base.DrawExtensionContainer(container);
        }
    }
}
