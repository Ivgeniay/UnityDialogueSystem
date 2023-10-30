using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public class PlusNode : BaseOperationNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Inputs.Add(new DialogueSystemInputModel(ID)
            {
                PortText = "A"
            });

            Inputs.Add(new DialogueSystemInputModel(ID)
            {
                PortText = "B",
            });

            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
            });
        }

        protected override BasePort CreateInputPort(object userData)
        {
            DialogueSystemInputModel choiceData = userData as DialogueSystemInputModel;

            BasePort inputPort = this.CreatePort(
                choiceData.PortText,
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Single);

            return inputPort;
        }
        protected override BasePort CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            BasePort inputPort = this.CreatePort(
                "",
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Multi);

            return inputPort;
        }

        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            base.OnConnectInputPort(port, edge);

            var inpPorts = GetInputPorts();
            bool allInputPortsConnected = inpPorts
                .Where(inputPort => inputPort != port)
                .All(inputPort => inputPort.connections.Any());

            if (allInputPortsConnected)
            {
                List<object> values = new List<object>();
                foreach (BasePort inputPort in inpPorts)
                {
                    if(inputPort == port)
                    {
                        BasePort connectedPort = edge.output as BasePort;
                        if (connectedPort != null && connectedPort.Value != null)
                        {
                            object convertedValue = Convert.ChangeType(connectedPort.Value, connectedPort.portType);
                            values.Add(convertedValue);
                        }
                    }
                    else
                    {
                        foreach (Edge _edge in inputPort.connections)
                        {
                            BasePort connectedPort = _edge.output as BasePort;
                            if (connectedPort != null && connectedPort.Value != null)
                            {
                                object convertedValue = Convert.ChangeType(connectedPort.Value, connectedPort.portType);
                                values.Add(convertedValue);
                            }
                        }
                    }
                }

                if (values.Count > 0)
                {
                    object sum = values.Sum(value => Convert.ToDouble(value));
                    var outputs = GetOutputPorts();
                    foreach (BasePort outputPort in outputs)
                        outputPort.Value = sum;
                    
                    Debug.Log("Сумма значений: " + sum);
                }
            }
        }

        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            base.OnDestroyConnectionInput(port, edge);
            var outputs = GetOutputPorts();
            foreach (BasePort outputPort in outputs)
                outputPort.Value = null;
            
        }
    }
}
