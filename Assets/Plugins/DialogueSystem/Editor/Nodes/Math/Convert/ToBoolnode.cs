using System.Collections.Generic;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    internal class ToBoolNode : BaseConvertNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Inputs.Add(new(ID)
            {
                PortText = $"All",
                Value = 0,
                Cross = false,
                IsField = false,
                IsInput = true,
                IsSingle = true,
                Type = typeof(bool)
            });

            Outputs.Add(new(ID)
            {
                Type = typeof(bool),
                Value = 0f,
                Cross = false,
                IsField = false,
                IsInput = false,
                IsSingle = false,
                PortText = typeof(bool).Name,
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
