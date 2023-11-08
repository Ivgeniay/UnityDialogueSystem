using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    internal class MoreOrEqual : BaseLogicNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new Database.Save.DSPortModel(DSConstants.NumberTypes)
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

                Model.Inputs.Add(new Database.Save.DSPortModel(DSConstants.NumberTypes)
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

                Model.Outputs.Add(new Database.Save.DSPortModel(new Type[]
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

        public override void Do(PortInfo[] portInfos)
        {
            base.Do(portInfos);

            //if (values.Count > 1)
            //{
            //    var doub1 = Convert.ToDouble(values[0]);
            //    var doub2 = Convert.ToDouble(values[1]);
            //    var output = GetOutputPorts()[0];
            //    output.Value = doub1 >= doub2;
            //    Debug.Log($"{values[0]} is more or equals than {values[1]} = {output.Value}");
            //}
        }
    }
}