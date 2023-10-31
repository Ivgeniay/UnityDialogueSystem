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
            Outputs.Add(new DialogueSystemPortModel(ID)
            {
                Value = 0,
                Type = typeof(int),
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = typeof(int).Name,
            });

            Model.Value = 0;
        }
    }
}
