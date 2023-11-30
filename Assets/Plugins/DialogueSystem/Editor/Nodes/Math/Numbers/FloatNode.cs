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
    internal class FloatNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(new Type[] { typeof(float) }, PortSide.Output)
                {
                    Value = "0",
                    Type = typeof(float),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    PortSide = PortSide.Output,
                    IsSingle = false,
                    PortText = DSConstants.Float,
                    IsAnchorable = true,
                });
            }
        }

    }
}
