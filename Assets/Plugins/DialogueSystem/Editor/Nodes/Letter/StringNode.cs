using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Database.Save;
using System.Threading.Tasks;
using UnityEngine;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal class StringNode : LetterNodeBase
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
                Value = string.Empty,
                PortType = typeof(string)
            });
        }

        protected override BasePort CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            BasePort outputPort = this.CreatePort(
                choiceData.PortType.Name,
                orientation: Orientation.Horizontal,
                direction: Direction.Output,
                capacity: Port.Capacity.Multi,
                type: choiceData.PortType);

            TextField Text = DialogueSystemUtilities.CreateTextField(
                (string)choiceData.Value,
                onChange: callback =>
                {
                    TextField target = callback.target as TextField;
                    target.value = callback.newValue;
                    choiceData.Value = callback.newValue.ToString();
                    outputPort.Value = callback.newValue.ToString();
                },
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__choice-textfield",
                        "ds-node__textfield__hidden"
                    }
                );

            outputPort.Add(Text);

            return outputPort;
        }
    }
}
