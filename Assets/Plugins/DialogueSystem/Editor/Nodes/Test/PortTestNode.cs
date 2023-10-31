using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Window;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal class PortTestNode : TestNodes
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            Outputs.Add(new(ID, new System.Type[] { typeof(string) })
            {
                Type = typeof(string),
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = "",
                Value = "New choice"
            });
        }

        protected override void DrawOutputContainer(VisualElement container)
        {
            base.DrawOutputContainer(container);

            var port = container.Children().ToList()[0] as BasePort;
            var t = AddPortByType(
                type: typeof(bool),
                cross: false,
                isField: false,
                isInput: true,
                isSingle: false,
                portText: "If",
                value: false,
                availableTypes: new System.Type[]
                {
                    typeof(bool),
                });

            inputContainer.Remove(t.port);
            port.Add(t.port);
        }
    }
}
