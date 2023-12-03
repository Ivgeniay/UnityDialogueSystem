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
                Model.AddPort(new DSPortModel(DSConstants.AllTypes, PortSide.Output)
                {
                    Type = typeof(Type),
                    PortText = "Instance",
                    IsSingle = false,
                });

                Model.AddPort(new DSPortModel(DSConstants.AllTypes, PortSide.Input)
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
            bool continues = BasePortManager.HaveCommonTypes(connectedPort.Type, port.AvailableTypes);
            if (!continues) return;

            if (connectedPort != null)
            {
                if (objectField.Type == null || !objectField.Type.Equals(connectedPort.Type))
                {
                    if (objectField.Value != null) objectField.Value = null;
                    objectField.Type = connectedPort.Type;
                    ChangeOutputPortsTypeAndName(connectedPort.Type, "Instance");
                }
            }
            objectField.SetEnabled(objectField.objectType != null);
        }

        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            base.OnDestroyConnectionInput(port, edge);
            objectField.SetEnabled(false);
            objectField.Value = null;
            objectField.Type = null;
            ChangeOutputPortsTypeAndName(typeof(Type), "Instance"); 
        }

        protected override void DrawExtensionContainer(VisualElement container)
        {
            objectField = new();
            objectField.label = "Instance";
            objectField.SetEnabled(objectField.value != null); 

            Assembly assembly = Assembly.Load(DSConstants.DEFAULT_ASSEMBLY);
            Type[] publicTypes = assembly.GetExportedTypes();

            BasePort input = GetInputPorts().First();
            BasePort output = GetOutputPorts().First();

            if (output != null) 
            {
                objectField.Type = output.Type;
                objectField.Value = output.AssetSource;
                ChangeOutputPortsTypeAndName(output.Type, "Instance");
            }

            objectField.RegisterValueChangedCallback<UnityEngine.Object>(e =>
            {
                var model = Model.Outputs.FirstOrDefault(e => e.PortID == output.ID);
                model.AssetSource = e.newValue;
                output.AssetSource = e.newValue;
                output.SetValue(output.AssetSource);
                graphView.OnValidate();
            });

            container.Add(objectField);
            base.DrawExtensionContainer(container);
            InitializeSettingElement(container);
        } 

        public (Type, object) GetValue() => (objectField.Type, objectField.Value);
        
    }
}
