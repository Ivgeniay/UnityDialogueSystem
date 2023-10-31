using DialogueSystem.Database.Save;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class MultiplyNode : BaseOperationNode
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

                    List<double> doubles = new List<double>();
                    foreach (var item in values)
                        doubles.Add(Convert.ToDouble(item));

                    double result = doubles[0];
                    if (doubles.Count > 1)
                    {
                        for (int i = 1; i < doubles.Count; i++)
                            result *= doubles[i];
                    }

                    var output = GetOutputPorts()[0];
                    output.Value = result;
                    Debug.Log("Mult значений: " + result);
                }
            }
            else
            {
                Debug.Log("Нет значений для обработки.");
            }
        }
    }
}
