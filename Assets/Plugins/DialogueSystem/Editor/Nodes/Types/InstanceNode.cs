using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.UIElement;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using System.Reflection; 
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace DialogueSystem.Nodes
{
    internal class InstanceNode : BaseTypeNode
    {
        protected DSObjectField objectField;

        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            base.Initialize(graphView, position, context);
            if (context == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.AllTypes, Ports.PortSide.Output)
                {
                    Type = typeof(Type),
                    PortText = "Instance",
                    IsSingle = false,
                });

                Model.AddPort(new DSPortModel(DSConstants.TypeTypes, PortSide.Input)
                {
                    Type= typeof(Type),
                    PortText = "Type",
                    Value = "",
                    IsSingle = true,
                });
            }
        }

        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            base.OnConnectInputPort(port, edge);

            BasePort connectedPort = edge.output as BasePort;
            bool continues = BasePortManager.HaveCommonTypes(connectedPort.AvailableTypes, port.AvailableTypes);
            if (!continues) return;

            if (connectedPort != null)
            {
                if (objectField.objectType == null || !objectField.objectType.Equals(connectedPort.Type))
                { 
                    if (objectField.value != null) objectField.value = null;
                    ChangeOutputPortsTypeAndName(connectedPort.Type, "Instance");
                    objectField.objectType = connectedPort.Type;
                }
            }
            objectField.SetEnabled(objectField.objectType != null);
        }

        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            base.OnDestroyConnectionInput(port, edge);
            objectField.SetEnabled(false);
            objectField.value = null;
            objectField.objectType = null;
            ChangeOutputPortsTypeAndName(typeof(Type), "Instance");
        }

        protected override void DrawExtensionContainer(VisualElement container)
        {
            objectField = new();
            objectField.label = "Instance: ";
            objectField.SetEnabled(objectField.value != null); 

            Assembly assembly = Assembly.Load(DSConstants.DEFAULT_ASSEMBLY);
            Type[] publicTypes = assembly.GetExportedTypes();

            BasePort input = GetInputPorts()[0];
            BasePort output = GetOutputPorts()[0];

            if (output != null) 
            {
                objectField.objectType = output.Type;
                objectField.value = output.AssetSource;
                ChangeOutputPortsTypeAndName(output.Type, "Instance");
            }

            objectField.RegisterValueChangedCallback<UnityEngine.Object>(e =>
            {
                var model = Model.Outputs.FirstOrDefault(e => e.PortID == output.ID);
                model.AssetSource = e.newValue;
                output.AssetSource = e.newValue;
                output.SetValue(output.AssetSource);
            });

            //foreach (Type type in publicTypes)
            //    dropdownTypes.choices.Add(type.FullName);

            //BasePort output = GetOutputPorts()[0];
            //if (output.Value != null)
            //{
            //    Type type = assembly.GetType(output.Value.ToString());
            //    if (type != null)
            //        dropdownTypes.value = type.ToString();
            //}

            //objectField.RegisterValueChangedCallback<string>((e) =>
            //{
            //    Type type = assembly.GetType(e.newValue);
            //    if (type != null)
            //    {
            //        this.ChangeOutputPortsTypeAndName(type);
            //        output.ChangeName(type.Name);
            //        Debug.Log(type);
            //    }
            //});

            container.Add(objectField);
            base.DrawExtensionContainer(container);
        }

    }
}
