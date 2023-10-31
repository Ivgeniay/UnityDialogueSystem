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
using UnityEngine.Windows;

namespace DialogueSystem.Nodes
{
    public abstract class BaseOperationNode : BaseMathNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Inputs.Add(new DialogueSystemPortModel(ID)
            {
                PortText = GetLetterFromNumber(Inputs.Count),
                Cross = false,
                IsField = false,
                IsInput = true,
                IsSingle = true,
                Type = typeof(bool),
                Value = false,
            });

            Inputs.Add(new DialogueSystemPortModel(ID)
            {
                PortText = GetLetterFromNumber(Inputs.Count),
                Cross = false,
                IsField = false,
                IsInput = true,
                IsSingle = true,
                Type = typeof(bool),
                Value = false,
            });

            Outputs.Add(new DialogueSystemPortModel(ID)
            {
                Value = 0,
                Cross = false,
                IsField = false,
                IsInput = false,
                IsSingle = true,
                Type = typeof(double),
            });
        }

        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            Button addChoiceBtn = DialogueSystemUtilities.CreateButton(
                "AddInput",
                () =>
                {
                    var t = AddPortByType(
                        portText: GetLetterFromNumber(this.Inputs.Count),
                        type: typeof(bool),
                        value: "",
                        isInput: true,
                        isSingle: true,
                        isField: false,
                        cross: true);
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