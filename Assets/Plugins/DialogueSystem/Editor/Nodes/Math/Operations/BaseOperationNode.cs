using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public abstract class BaseOperationNode : BaseMathNode
    {
        //private BasePort lastConnectedPort = null;
        //private Edge lastConnectedEdge = null;

        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Inputs.Add(new DialogueSystemInputModel(ID)
            {
                PortText = GetLetterFromNumber(Inputs.Count),
            });

            Inputs.Add(new DialogueSystemInputModel(ID)
            {
                PortText = GetLetterFromNumber(Inputs.Count),
            });

            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
                Value = 0
            });
        }

        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            Button addChoiceBtn = DialogueSystemUtilities.CreateButton(
                "AddInput",
                () =>
                {
                    DialogueSystemInputModel choiceData = new DialogueSystemInputModel(ID)
                    {
                        PortText = GetLetterFromNumber(Inputs.Count)
                    };
                    BasePort choicePort = CreateInputPort(choiceData);
                    choicePort.portName = choiceData.PortText;
                    Inputs.Add(choiceData);
                    inputContainer.Add(choicePort);
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );
            container.Insert(1, addChoiceBtn);
        }

        protected override BasePort CreateInputPort(object userData)
        {
            DialogueSystemInputModel choiceData = userData as DialogueSystemInputModel;

            BasePort inputPort = this.CreatePort(
                choiceData.PortText,
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Single,
                type: choiceData.Type);
            inputPort.Value = choiceData.Value;

            return inputPort;
        }
        protected override BasePort CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            BasePort outputPort = this.CreatePort(
                "Output",
                orientation: Orientation.Horizontal,
                direction: Direction.Output,
                capacity: Port.Capacity.Multi,
                type: choiceData.PortType);
            outputPort.Value = choiceData.Value;

            return outputPort;
        }

        //public override void OnConnectInputPort(BasePort port, Edge edge)
        //{
        //    base.OnConnectInputPort(port, edge);
        //    lastConnectedPort = port;
        //    lastConnectedEdge = edge;

        //    UpdateValue();
        //}
        //public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        //{
        //    base.OnDestroyConnectionInput(port, edge);
        //    var outputs = GetOutputPorts();
        //    foreach (BasePort outputPort in outputs)
        //        outputPort.Value = null;
        //}
        //public override void UpdateValue()
        //{
        //    base.UpdateValue();

        //    var inpPorts = GetInputPorts();
        //    List<BasePort> matchingPorts = new List<BasePort>();

        //    foreach (DialogueSystemInputModel input in Inputs)
        //    {
        //        foreach (BasePort port in inpPorts)
        //        {
        //            if (port.portName == input.PortText)
        //                matchingPorts.Add(port);
        //        }
        //    }

        //    if (matchingPorts.Count >= 2)
        //    {
        //        List<object> values = new List<object>();

        //        foreach (BasePort port in matchingPorts)
        //        {
        //            if (port.connections.Any())
        //            {
        //                BasePort connectedPort = port.connections.First().output as BasePort;
        //                if (connectedPort != null && connectedPort.Value != null)
        //                {
        //                    object convertedValue = Convert.ChangeType(connectedPort.Value, connectedPort.portType);
        //                    values.Add(convertedValue);
        //                }
        //            }
        //            if (!port.connections.Any() && port == lastConnectedPort)
        //            {
        //                BasePort connectedPort = lastConnectedEdge.output as BasePort;
        //                if (connectedPort != null && connectedPort.Value != null)
        //                {
        //                    object convertedValue = Convert.ChangeType(connectedPort.Value, connectedPort.portType);
        //                    values.Add(convertedValue);
        //                }
        //            }
        //        }
        //        if (values.Count > 0)
        //        {
        //            Do(values);
        //        }
        //    }
        //}
    }
}