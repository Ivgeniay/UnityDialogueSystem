using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class ToFloatNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    PortText = DSConstants.All,
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = false,
                    Type = typeof(bool),
                });

                Outputs.Add(new(new Type[] { typeof(float) })
                {
                    PortText = DSConstants.Float,
                    Type = typeof(float),
                    Value = 0f,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = false,
                });
            }
        }

        public override void Do(List<object> values)
        {
            base.Do(values);

            if (values.Count > 0)
            {
                float result = default;
                if (values[0] is null) result = 0f;
                else if (values[0] is double)
                    result = SafeConvertDoubleToFloat((double)values[0]);
                else if (values[0] is int)
                    result = ConvertIntToFloat((int)values[0]);
                else if (values[0] is string)
                    result = SafeParseToFloat((string)values[0]);
                else if (values[0] is bool)
                    result = ConvertBoolToFloat((bool)values[0]);
                
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
