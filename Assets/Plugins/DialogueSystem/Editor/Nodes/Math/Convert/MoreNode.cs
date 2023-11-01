using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class MoreNode : BaseLogicNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);

            if (portsContext == null)
            {
                Inputs.Add(new Database.Save.DialogueSystemPortModel(DialogueSystemConstants.ListNumberTypes)
                {
                    Type = typeof(double),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = "Number",
                    Value = 0
                });

                Inputs.Add(new Database.Save.DialogueSystemPortModel(new Type[]
                {
                    typeof(int),
                    typeof(float),
                    typeof(double)
                })
                {
                    Type = typeof(double),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = "Number",
                    Value = 0
                });

                Outputs.Add(new Database.Save.DialogueSystemPortModel(new Type[]
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
                    PortText = typeof(bool).Name,
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
                output.Value = doub1 > doub2;
                Debug.Log($"{values[0]} is more than {values[1]} = {output.Value}");
            }
        }
    }
}
