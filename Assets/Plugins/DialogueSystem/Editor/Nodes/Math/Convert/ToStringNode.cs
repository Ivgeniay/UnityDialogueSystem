using DialogueSystem.Ports;
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
    internal class ToStringNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    PortText = DSConstants.All,
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool)
                });

                Model.Outputs.Add(new(new Type[]
                {
                    typeof(string)
                })
                {
                    Type = typeof(string),
                    Value = 0f,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.String,
                });
            }
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
