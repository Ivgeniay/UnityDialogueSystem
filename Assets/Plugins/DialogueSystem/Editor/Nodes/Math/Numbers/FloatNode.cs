﻿using UnityEditor.Experimental.GraphView;
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
    public class FloatNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Outputs.Add(new DSPortModel(new Type[] { typeof(float) })
                {
                    Value = 0f,
                    Type = typeof(float),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Float,
                    IsSerializedInScript = true,
                });
            }
        }

    }
}
