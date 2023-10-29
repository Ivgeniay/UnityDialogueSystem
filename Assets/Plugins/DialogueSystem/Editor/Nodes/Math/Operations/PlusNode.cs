using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Window;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    public class PlusNode : BaseOperationNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Inputs.Add(new DialogueSystemInputModel(ID)
            {
            });

            Inputs.Add(new DialogueSystemInputModel(ID)
            {
            });

            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
            });
        }


        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            var tt = Inputs.Where(el => el.Port == port).FirstOrDefault();
            if (tt != null)
            {
                tt.NodeID = "";
            }
        }

        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            var tt = Inputs.Where(el => el.Port == port).FirstOrDefault();
            var output1 = edge.output.node as BaseNode;
            tt.NodeID = output1.Model.ID;

            var allConnections = Inputs.All(el => !string.IsNullOrEmpty(el.NodeID));

            if (allConnections)
            {
                foreach (var inputPortModel in Inputs)
                {
                    foreach (Edge ed in inputPortModel.Port.connections)
                    {
                        //Debug.Log("Соединение с портом " + inputPort.portName + ": " + edge);
                    }
                }
            }
        }

    }
}
