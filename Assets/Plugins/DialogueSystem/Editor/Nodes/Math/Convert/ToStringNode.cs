using DialogueSystem.Ports;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class ToStringNode : BaseConvertNode
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
                Type = typeof(string),
                Value = 0f,
                Cross = false,
                IsField = false,
                IsInput = false,
                IsSingle = false,
                PortText = typeof(float).Name,
            });
        }

        public override void Do(List<object> values)
        {
            base.Do(values);

            if (values.Count > 0)
            {
                string result = values[0].ToString();

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
