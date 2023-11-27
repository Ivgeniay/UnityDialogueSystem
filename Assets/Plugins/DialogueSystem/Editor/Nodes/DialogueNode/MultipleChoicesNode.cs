﻿using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System.Collections.Generic;
using System;
using DialogueSystem.Ports;

namespace DialogueSystem.Nodes
{
    internal class MultipleChoicesNode : BaseDialogueNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.DialogueTypes, Ports.PortSide.Output)
                {
                    Value = "Next Choice",
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = true,
                    PlusIf = true,
                    PortText = DSConstants.Dialogue,
                    Type = DSConstants.DialogueTypes[0]
                });
            }
        }
        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            Button addChoiceBtn = DSUtilities.CreateButton(
                "Add Choice", 
                () =>
                {
                    var t = AddPortByType(
                        ID: Guid.NewGuid().ToString(),
                        portText: DSConstants.Dialogue,
                        type: DSConstants.DialogueTypes[0],
                        value: "Choice",
                        isInput: false,
                        isSingle: true,
                        isField: true,
                        cross: true,
                        plusIf: true,
                        portSide: PortSide.Output,
                        availableTypes: DSConstants.DialogueTypes
                        );
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
