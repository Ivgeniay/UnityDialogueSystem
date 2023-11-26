using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseLogicNode : BaseNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new Database.Save.DSPortModel(new Type[]
                {
                    typeof(bool)
                }, Ports.PortSide.Output)
                {
                    Type = typeof(bool),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = false,
                    IsSingle = true,
                    PortText = DSConstants.Bool,
                    Value = false,
                    IsFunction = true,
                    IsSerializedInScript = true,
                });
            }
        }
    }
}
