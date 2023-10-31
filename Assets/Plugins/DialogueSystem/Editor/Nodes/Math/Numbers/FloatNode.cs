using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System;
using DialogueSystem.Ports;
using System.Collections.Generic;

namespace DialogueSystem.Nodes
{
    public class FloatNode : BaseNumbersNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Outputs.Add(new DialogueSystemPortModel(new Type[] { typeof(float) })
                {
                    Value = 0f,
                    Type = typeof(float),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = typeof(float).Name,
                });
            }
            Model.Value = 0;
        }

    }
}
