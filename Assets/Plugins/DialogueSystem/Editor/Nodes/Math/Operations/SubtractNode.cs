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
    internal class SubtractNode : BaseOperationNode
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

                List<string> strings = new List<string>();
                values.ForEach(value =>
                {
                    if (value != null) 
                        strings.Add(value.ToString());
                });
                string result = strings[0];

                if (strings.Count > 1)
                {
                    for (int i = 1; i < strings.Count; i++)
                    {
                        int index = result.IndexOf(strings[i]);
                        if (index >= 0)
                            result = result.Remove(index, strings[i].Length);
                    }
                }

                output.Value = result;

                Debug.Log("Результат отнятия строк: " + result);
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
                        result -= doubles[i];
                }

                output.Value = result;
                Debug.Log("Разница значений: " + result);
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
                if (i != inputVariables.Length - 1) sb.Append(" - ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
