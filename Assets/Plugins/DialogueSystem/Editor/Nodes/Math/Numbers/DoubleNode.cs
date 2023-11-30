using DialogueSystem.Generators;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System.Linq;
using System.Text;
using UnityEngine;
using System;
using DialogueSystem.Database.Save;
using DialogueSystem.Ports;

namespace DialogueSystem.Nodes
{
    internal class DoubleNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(new Type[] { typeof(double) }, PortSide.Output)
                {
                    Value = "0",
                    Type = typeof(double),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Double,
                    IsAnchorable = true,
                });
            }
        }

    }
}
