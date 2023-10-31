using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    public class AdditionNode : BaseOperationNode
    {
        public override void Do(List<object> values)
        {
            if (values.Count > 0)
            {
                bool containsString = values.Any(value => value is string);

                if (containsString)
                {
                    ChangeOutputPortType(typeof(string));
                    string concatenatedValue = string.Join("", values);
                    var output = GetOutputPorts()[0];
                    output.Value = concatenatedValue;
                    Debug.Log("Результат конкатенации: " + concatenatedValue);
                }
                else
                {
                    ChangeOutputPortType(typeof(double));
                    double sum = values.Sum(value => Convert.ToDouble(value));
                    var output = GetOutputPorts()[0];
                    output.Value = sum;
                    Debug.Log("Сумма значений: " + sum);
                }
            }
            else
            {
                Debug.Log("Нет значений для обработки.");
            }
        }
    }
}
