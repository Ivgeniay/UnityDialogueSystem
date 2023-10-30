using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System;
using DialogueSystem.Ports;

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
                PortType = typeof(float),
            });

            Model.Value = 0;
        }

        protected override BasePort CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;
            
            var choicePort = base.CreateOutputPort(userData);

            FloatField floatField = DialogueSystemUtilities.CreateFloatField(
                0,
                onChange: callback =>
                {
                    FloatField target = callback.target as FloatField;
                    target.value = callback.newValue;
                    choiceData.Value = callback.newValue;
                    Model.Value = callback.newValue;
                    choicePort.Value = callback.newValue;
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
