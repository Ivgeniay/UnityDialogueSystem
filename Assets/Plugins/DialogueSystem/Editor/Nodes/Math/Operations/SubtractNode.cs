using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class SubtractNode : BaseOperationNode
    {
        public override void Do(List<object> values)
        {
            if (values.Count > 0)
            {
                bool containsString = values.Any(value => value is string);

                if (containsString)
                {
                    ChangeOutputPortType(typeof(string));

                    List<string> strings = new List<string>();
                    values.ForEach(value => strings.Add(value.ToString()));
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

                    var output = GetOutputPorts()[0];
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

                    var output = GetOutputPorts()[0];
                    output.Value = result;
                    Debug.Log("Разница значений: " + result);
                }
            }
            else
            {
                Debug.Log("Нет значений для обработки.");
            }
        }
    }
}
