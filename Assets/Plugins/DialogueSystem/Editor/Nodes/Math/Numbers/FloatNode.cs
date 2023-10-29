﻿using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    public class FloatNode : BaseNumbersNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
                Value = 0f,
            });

            Model.Value = 0;
        }

        protected override Port CreateOutputPort(object userData)
        {
            var choicePort = base.CreateOutputPort(userData);
            choicePort.portType = typeof(float);

            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel; 

            FloatField floatField = DialogueSystemUtilities.CreateFloatField(
                0,
                onChange: callback =>
                {
                    FloatField target = callback.target as FloatField;
                    target.value = callback.newValue;
                    choiceData.Value = callback.newValue;
                    Model.Value = callback.newValue;
                },
                styles: new string[]
                    {
                        "ds-node__floatfield",
                        "ds-node__choice-textfield",
                        "ds-node__textfield__hidden"
                    }
                );

            choicePort.Add(floatField);
            return choicePort;
        }
    }
}
