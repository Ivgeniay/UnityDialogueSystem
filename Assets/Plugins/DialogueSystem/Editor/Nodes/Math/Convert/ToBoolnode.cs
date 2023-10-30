using System.Collections.Generic;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using UnityEngine;
using UnityEngine.Windows;
using System;

namespace DialogueSystem.Nodes
{
    internal class ToBoolnode : BaseConvertNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Inputs.Add(new(ID)
            {
                PortText = $"{typeof(float).Name}/{typeof(double).Name}/{typeof(string).Name}/{typeof(int).Name}/{typeof(bool).Name}",
                Value = 0,
            });

            Outputs.Add(new(ID)
            {
                PortType = typeof(bool),
                Value = 0f,
            });
        }

        public override void Do(List<object> values)
        {
            base.Do(values);

            if (values.Count > 0)
            {
                bool result = default;
                if (values[0] is null) result = false;
                else if (values[0] is double doub)
                    result = Convert.ToBoolean(doub);
                else if (values[0] is float fl)
                    result = Convert.ToBoolean(fl);
                else if (values[0] is int integer)
                    result = Convert.ToBoolean(integer);
                else if (values[0] is string str)
                {
                    if (bool.TryParse(str, out result)) { }
                }
                else if (values[0] is bool b)
                    result = b;

                BasePort output = GetOutputPorts()[0];
                output.Value = result;
                Debug.Log($"Преобразованное значения: из {values[0]} {values[0].GetType().Name} в {result} {result.GetType().Name}");
            }
            else
            {
                Debug.Log("Нет значений для обработки.");
            }
        }
    }
}
