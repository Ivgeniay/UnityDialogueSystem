﻿using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class EqualNode : BaseLogicNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Inputs.Add(new(DialogueSystemUtilities.ListAvalilableTypes.ToArray())
                {
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = "All",
                    Type = null,
                    Value = null,
                });

                Inputs.Add(new(DialogueSystemUtilities.ListAvalilableTypes.ToArray())
                {
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = "All",
                    Type = null,
                    Value = null,
                });

                Outputs.Add(new(new Type[] { typeof(bool) })
                {
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = typeof(bool).Name,
                    Type = typeof(bool),
                    Value = false,
                });
            }
        }
        public override void Do(List<object> values)
        {
            base.Do(values);

            if (!values.Any(e => e == null) && values.Count > 1)
            {
                var output = GetOutputPorts()[0];
                output.Value = values[0].ToString() == values[1].ToString();
                Debug.Log($"{values[0]} equals {output.Value} {values[1]}");
            }
        }
    }
}
