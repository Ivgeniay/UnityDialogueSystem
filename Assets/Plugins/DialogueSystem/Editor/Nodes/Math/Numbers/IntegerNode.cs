using DialogueSystem.Database.Save;
using DialogueSystem.Window;
using UnityEngine;
using System;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Ports;

namespace DialogueSystem.Nodes
{
    public class IntegerNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(new Type[] { typeof(int) }, PortSide.Output)
                {
                    Value = 0,
                    Type = typeof(int),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Int,
                    IsSerializedInScript = true,
                });
            }
        }
    }
}
