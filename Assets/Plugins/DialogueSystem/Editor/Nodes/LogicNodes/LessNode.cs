﻿using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class LessNode : BaseLogicNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);

            if (portsContext == null)
            {
                Inputs.Add(new Database.Save.DSPortModel(DSConstants.NumberTypes)
                {
                    Type = typeof(double),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = DSConstants.Number,
                    Value = 0
                });

                Inputs.Add(new Database.Save.DSPortModel(DSConstants.NumberTypes)
                {
                    Type = typeof(double),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = DSConstants.Number,
                    Value = 0
                });

                Outputs.Add(new Database.Save.DSPortModel(new Type[]
                {
                    typeof(bool)
                })
                {
                    Type = typeof(bool),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Bool,
                    Value = 0
                });
            }
        }

        public override void Do(List<object> values)
        {
            base.Do(values);

            if (values.Count > 1)
            {
                var doub1 = Convert.ToDouble(values[0]);
                var doub2 = Convert.ToDouble(values[1]);
                var output = GetOutputPorts()[0];
                output.Value = doub1 < doub2;
                Debug.Log($"{values[0]} is less than {values[1]} = {output.Value}");
            }
        }
    }
}