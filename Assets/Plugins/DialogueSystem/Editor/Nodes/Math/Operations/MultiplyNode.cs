using DialogueSystem.Database.Save;
using DialogueSystem.Generators;
using DialogueSystem.Ports;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class MultiplyNode : BaseOperationNode
    {
        public override void Do(List<object> values)
        {
            BasePort output = GetOutputPorts()[0];
            if (values == null || values.Count == 0)
            {
                ChangePort(output, typeof(double));
                output.Value = 0d;
                return;
            }

            List<BasePort> inputs = GetInputPorts();

            bool containsString = values.Any(value => value is string);
            if (containsString)
            {
                ChangeOutputPortType(typeof(string));

                string concatenatedValue = string.Join("", values);
                output.Value = concatenatedValue;
                Debug.Log("Результат конкатенации: " + concatenatedValue);
            }
            else
            {
                ChangeOutputPortType(typeof(double));

                List<double> doubles = new List<double>();
                foreach (var item in values)
                    doubles.Add(Convert.ToDouble(item));

                double result = doubles[0];
                if (doubles.Count > 1)
                {
                    for (int i = 1; i < doubles.Count; i++)
                        result *= doubles[i];
                }

                output.Value = result;
                Debug.Log("Mult значений: " + result);
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] != null)
                    ChangePort(inputs[i], values[i].GetType());
            }
        }

        internal override string MethodGenerationContext(MethodGen.MethodParamsInfo[] inputVariables, MethodGen.MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                sb.Append($"{inputVariables[i].ParamName}");
                if (i != inputVariables.Length - 1) sb.Append(" * ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
