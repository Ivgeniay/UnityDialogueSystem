using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class PortTestNode : TestNodes
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Outputs.Add(new(new System.Type[] { typeof(string) })
                {
                    Cross = true,
                    IsField = true,
                    IsIfPort = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = typeof(string).Name,
                    Type = typeof(string),
                    Value = "kuku"
                });

                Model.Inputs.Add(new(new System.Type[]
                {
                    typeof(string),
                    typeof(bool),
                })
                {
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = false,
                    PortText = "(String)(Bool)",
                    Type = typeof(string),
                    IsIfPort = false,
                });

                Model.Inputs.Add(new(new System.Type[] { typeof(bool) })
                {
                    Cross = false,
                    IsField = false,
                    IsIfPort = true,
                    IsInput = true,
                    IsSingle = true,
                    PortText = "If (bool)",
                    Type = typeof(bool),
                    Value = false
                });
            }
        }

        //protected override void DrawOutputContainer(VisualElement container)
        //{
        //    base.DrawOutputContainer(container);

        //    var port = container.Children().ToList()[0] as BasePort;
        //    var t = AddPortByType(
        //        type: typeof(bool),
        //        cross: false,
        //        isField: false,
        //        isInput: true,
        //        isSingle: false,
        //        portText: "If",
        //        value: false,
        //        availableTypes: new System.Type[]
        //        {
        //            typeof(bool),
        //        });

        //    inputContainer.Remove(t.port);
        //    port.Add(t.port);
        //}
    }
}
