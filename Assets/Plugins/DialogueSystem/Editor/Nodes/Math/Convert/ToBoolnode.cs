using System.Collections.Generic;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using UnityEngine;
using System;
using DialogueSystem.Utilities;

namespace DialogueSystem.Nodes
{
    internal class ToBoolNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    PortText = $"All",
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool)
                });

                Model.Outputs.Add(new(new Type[]
                {
                    typeof(bool)
                })
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
        }

        public override void Do(List<object> values)
        {
            base.Do(values);

            BasePort output = GetOutputPorts()[0];

            if (values == null || values.Count == 0)
            {
                ChangePort(output, typeof(bool));
                output.Value = false;
                return;
            }

            List<BasePort> inputs = GetInputPorts();

            if (values.Count > 0)
            {
                bool result = default;
                if (values[0] is null) result = false;
                else if (values[0] is double doub)
                {
                    result = Convert.ToBoolean(doub);
                }
                else if (values[0] is float fl)
                {
                    result = Convert.ToBoolean(fl);
                }
                else if (values[0] is int integer)
                {
                    result = Convert.ToBoolean(integer);
                }
                else if (values[0] is string str)
                {
                    if (bool.TryParse(str, out result)) 
                    {
                        result = Convert.ToBoolean(result);
                    }
                }
                else if (values[0] is bool b)
                {
                    result = b;
                }

                if (values[0] is not null)
                {
                    ChangePort(inputs[0], values[0].GetType());
                }

                output.Value = result;
                Debug.Log($"Преобразованное значения: из {values[0]} {values[0].GetType().Name} в {result} {result.GetType().Name}");
            }
            else
            {
                Debug.Log("Нет значений для обработки.");
            }
        }

        //public override void Do(List<object> values)
        //{
        //    base.Do(values);

        //    BasePort output = GetOutputPorts()[0];

        //    if (values == null || values.Count < 2 || values[0] == null || values[1] == null)
        //    {
        //        ChangePort(output, typeof(bool));
        //        output.Value = false;
        //    }

        //    List<BasePort> inputs = GetInputPorts();

        //    if (values.Count > 0)
        //    {
        //        bool b1 = false;
        //        bool b2 = false;
        //        if (values[0] is bool _b1)
        //        {
        //            ChangePort(inputs[0], typeof(bool));
        //            b1 = _b1;
        //        }
        //        if (values[1] is bool _b2)
        //        {
        //            ChangePort(inputs[1], typeof(bool));
        //            b2 = _b2;
        //        }
        //        ChangePort(output, typeof(bool));
        //        output.Value = b1 == true && b2 == true;
        //    }
        //}
    }
}
