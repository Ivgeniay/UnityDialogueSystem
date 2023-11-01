using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    public abstract class BaseOperationNode : BaseMathNode
    {
        protected void ChangeOutputPortType(Type type)
        {
            var outs= GetOutputPorts();
            if (outs != null)
            {
                foreach (var outPort in outs)
                {
                    outPort.ChangeType(type);
                    outPort.ChangeName(type.Name);
                }
            }
        }

        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new DSPortModel(DSConstants.AvalilableTypes)
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count) + DSConstants.All,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool),
                    Value = false,
                });

                Model.Inputs.Add(new DSPortModel(DSConstants.AvalilableTypes)
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count) + DSConstants.All,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool),
                    Value = false,
                });

                Model.Outputs.Add(new DSPortModel(new Type[]
                {
                typeof(double),
                typeof(string),
                })
                {
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = true,
                    Type = typeof(double),
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
                        availableTypes: new Type[]
                        {
                            typeof(string),
                            typeof(int),
                            typeof(float),
                            typeof(double),
                            typeof(bool),
                        });
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );
            container.Insert(1, addChoiceBtn);
        }


    }
}