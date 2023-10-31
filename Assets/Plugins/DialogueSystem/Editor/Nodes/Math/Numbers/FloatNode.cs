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
            Outputs.Add(new DialogueSystemPortModel(ID)
            {
                Value = 0f,
                Type = typeof(float),
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = typeof(float).Name,
            });

            Model.Value = 0;
        }

    }
}
