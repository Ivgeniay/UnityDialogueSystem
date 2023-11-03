using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using DialogueSystem.Ports;

namespace DialogueSystem.Nodes
{
    public class AdditionNode : BaseOperationNode
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
                double sum = values.Sum(value => Convert.ToDouble(value));
                output.Value = sum;
                Debug.Log("Сумма значений: " + sum);
            }

            for (int i = 0; i < values.Count; i++ )
            {
                if (values[i] != null)
                    ChangePort(inputs[i], values[i].GetType());
            }

        }
    }
}
