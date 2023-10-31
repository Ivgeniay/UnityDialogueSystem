using DialogueSystem.Ports;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class IfNode : BaseLogicNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext != null )
            {
                Inputs.Add(new(new Type[] { typeof(bool) })
                {
                    PortText = GetLetterFromNumber(Inputs.Count),
                    Value = false,
                    Type = typeof(bool),
                    IsInput = true,
                    IsField = false,
                    IsSingle = true,
                    Cross = false,
                });

                Inputs.Add(new(new Type[] { typeof(bool) })
                {
                    PortText = GetLetterFromNumber(Inputs.Count),
                    Value = false,
                    Type = typeof(bool),
                    IsInput = true,
                    IsField = false,
                    IsSingle = true,
                    Cross = false,
                });

                Outputs.Add(new(new Type[] { typeof(bool) })
                {
                    PortText = GetLetterFromNumber(Inputs.Count),
                    Value = false,
                    Type = typeof(bool),
                    IsInput = false,
                    IsField = false,
                    IsSingle = false,
                    Cross = false,
                });
            }
        }

        //public override void Do(List<object> values)
        //{
        //    base.Do(values);

        //    if (values.Count > 0)
        //    {
        //        bool result = default;
        //        if (values[0] is null) result = false;
        //        else if (values[0] is double doub) result = Convert.ToBoolean(doub);
        //        else if (values[0] is float fl) result = Convert.ToBoolean(fl);
        //        else if (values[0] is int integer) result = Convert.ToBoolean(integer);
        //        else if (values[0] is string str)
        //        {
        //            if (bool.TryParse(str, out result)) { }
        //        }
        //        else if (values[0] is bool b) result = b;

        //        BasePort output = GetOutputPorts()[0];
        //        output.Value = result;
        //        Debug.Log($"Преобразованное значения: из {values[0]} {values[0].GetType().Name} в {result} {result.GetType().Name}");
        //    }
        //    else
        //    {
        //        Debug.Log("Нет значений для обработки.");
        //    }
        //}
    }
}
