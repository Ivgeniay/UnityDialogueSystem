using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class IfNode : BaseLogicNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext != null )
            {
                Model.Inputs.Add(new(new Type[] { typeof(bool) })
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count),
                    Value = false,
                    Type = typeof(bool),
                    IsInput = true,
                    IsField = false,
                    IsSingle = true,
                    Cross = false,
                });

                Model.Inputs.Add(new(new Type[] { typeof(bool) })
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count),
                    Value = false,
                    Type = typeof(bool),
                    IsInput = true,
                    IsField = false,
                    IsSingle = true,
                    Cross = false,
                });

                Model.Outputs.Add(new(new Type[] { typeof(bool) })
                {
                    PortText = GetLetterFromNumber(Model.Inputs.Count),
                    Value = false,
                    Type = typeof(bool),
                    IsInput = false,
                    IsField = false,
                    IsSingle = false,
                    Cross = false,
                });
            }
        }

        public override void Do(List<object> values)
        {
            base.Do(values);

            BasePort output = GetOutputPorts()[0];

            if (values == null || values.Count < 2 || values[0] == null || values[1] == null)
            {
                ChangePort(output, typeof(bool));
                output.Value = false;
            }

            List<BasePort> inputs = GetInputPorts();

            if (values.Count > 0)
            {
                bool b1 = false;
                bool b2 = false;
                if (values[0] is bool _b1)
                {
                    ChangePort(inputs[0], typeof(bool));
                    b1 = _b1;
                }
                if (values[1] is bool _b2)
                {
                    ChangePort(inputs[1], typeof(bool));
                    b2 = _b2;
                }
                ChangePort(output, typeof(bool));
                output.Value = b1 == true && b2 == true;
            }
        }
    }
}
