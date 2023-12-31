﻿using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class BoolNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);
            
            Model.Outputs = new();
            if (portsContext == null)
            {
                Model.Outputs.Add(new(new System.Type[]
                {
                    typeof(bool)
                })
                {
                    PortText = DSConstants.Bool,
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    Type = typeof(bool),
                    Value = false,
                    IsSerializedInScript = true,
                });
            }
        }
    }
}
