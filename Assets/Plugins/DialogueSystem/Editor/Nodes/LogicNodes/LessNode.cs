using DialogueSystem.Generators;
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
    internal class LessNode : BaseLogicNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new Database.Save.DSPortModel(DSConstants.NumberTypes, Ports.PortSide.Input)
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

                Model.AddPort(new Database.Save.DSPortModel(DSConstants.NumberTypes, Ports.PortSide.Input)
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
            }
        }

        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                sb.Append($"Convert.ToDouble({inputVariables[i].ParamName})");
                if (i != inputVariables.Length - 1) sb.Append(" < ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}