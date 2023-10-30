using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public class IntegerNode : BaseNumbersNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
                Value = 0,
                PortType = typeof(int)
            });

            Model.Value = 0;
        }

        protected override BasePort CreateOutputPort(object userData)
        {
            var choicePort = base.CreateOutputPort(userData);
            choicePort.portType = typeof(int);

            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            IntegerField integetField = DialogueSystemUtilities.CreateIntegerField(
                0,
                onChange: callback =>
                {
                    IntegerField target = callback.target as IntegerField;
                    target.value = callback.newValue;
                    choiceData.Value = callback.newValue;
                    Model.Value = callback.newValue;
                    choicePort.Value = callback.newValue;
                },
                styles: new string[]
                    {
                        "ds-node__integerfield",
                        "ds-node__choice-textfield",
                        "ds-node__textfield__hidden"
                    }
                );

            choicePort.Add(integetField);
            return choicePort;
        }
    }
}
