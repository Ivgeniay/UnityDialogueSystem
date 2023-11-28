using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System;
using DialogueSystem.Ports;
using System.Linq;

namespace DialogueSystem.Nodes
{
    public abstract class BaseOperationNode : BaseMathNode
    {

        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.AvalilableTypes, PortSide.Input)
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count) + DSConstants.All,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(double),
                    Value = "0",
                    IsAnchorable = true,
                });

                Model.AddPort(new DSPortModel(DSConstants.AvalilableTypes, PortSide.Input)
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count) + DSConstants.All,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(double),
                    Value = "0",
                    IsAnchorable = true,
                });

                Model.AddPort(new DSPortModel(new Type[] { typeof(double), typeof(string), }, PortSide.Output)
                {
                    Value = "0",
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = true,
                    Type = typeof(double),
                    IsFunction = true,
                    PortText = typeof(double).Name,
                    IsAnchorable = true,
                });
            }
        }

        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            Button addChoiceBtn = DSUtilities.CreateButton(
                "AddInput",
                () =>
                {
                    var t = AddPortByType(
                        ID: Guid.NewGuid().ToString(),
                        portText: GetLetterFromNumber(this.Model.Inputs.Count),
                        type: typeof(float),
                        value: 0,
                        isInput: true,
                        isSingle: true,
                        isField: false,
                        cross: true,
                        portSide: PortSide.Input,
                        availableTypes: new Type[]
                        {
                            typeof(string),
                            typeof(int),
                            typeof(float),
                            typeof(double),
                            typeof(bool),
                        },
                        isAnchorable: true);
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );
            container.Insert(1, addChoiceBtn);
        }

        public override void Do(PortInfo[] portInfos)
        {
            base.Do(portInfos);

            BasePort output = GetOutputPorts()[0];

            bool isStr = portInfos.Any(e => e.port.Type == typeof(string));
            ChangeOutputPortType(isStr == true ? typeof(string) : typeof(double));
        }

    }
}