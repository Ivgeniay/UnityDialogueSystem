using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Ports;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Globalization;

namespace DialogueSystem.Nodes
{
    public abstract class BaseMathNode : BaseNode
    {
        protected BasePort lastConnectedPort = null;
        protected Edge lastConnectedEdge = null;
        public override void UpdateValue()
        {
            base.UpdateValue();

            var inpPorts = GetInputPorts();
            List<BasePort> matchingPorts = new List<BasePort>();

            foreach (DialogueSystemInputModel input in Inputs)
            {
                foreach (BasePort port in inpPorts)
                {
                    if (port.portName == input.PortText)
                        matchingPorts.Add(port);
                }
            }

            //if (matchingPorts.Count >= 2)
            //{
                List<object> values = new List<object>();

                foreach (BasePort port in matchingPorts)
                {
                    if (port.connections.Any())
                    {
                        BasePort connectedPort = port.connections.First().output as BasePort;
                        if (connectedPort != null && connectedPort.Value != null)
                        {
                            object convertedValue = Convert.ChangeType(connectedPort.Value, connectedPort.portType);
                            values.Add(convertedValue);
                        }
                    }
                    if (!port.connections.Any() && port == lastConnectedPort)
                    {
                        BasePort connectedPort = lastConnectedEdge.output as BasePort;
                        if (connectedPort != null && connectedPort.Value != null)
                        {
                            object convertedValue = Convert.ChangeType(connectedPort.Value, connectedPort.portType);
                            values.Add(convertedValue);
                        }
                    }
                }
                if (values.Count > 0)
                {
                    Do(values);
                }
            //}
        }
        public virtual void Do(List<object> values) { }
        public virtual void ChangeOutputPortType(Type type)
        {
            var ports = outputContainer.Children();
            foreach (var port in ports)
            {
                if (port is BasePort bport)
                {
                    bport.ChangeType(type);
                    bport.ChangeName(type.Name);
                }
            }
        }

        public virtual double StringToDouble(string str)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            return 0;
        }

        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            base.OnConnectInputPort(port, edge);
            lastConnectedPort = port;
            lastConnectedEdge = edge;

            UpdateValue();
        }
        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            base.OnDestroyConnectionInput(port, edge);
            var outputs = GetOutputPorts();
            foreach (BasePort outputPort in outputs)
                outputPort.Value = null;
        }

        public float SafeConvertDoubleToFloat(double input)
        {
            if (input > float.MaxValue)
            {
                return float.MaxValue;
            }
            else if (input < float.MinValue)
            {
                return float.MinValue;
            }
            else
            {
                return (float)input;
            }
        }
        public double SafeParseToDouble(string input)
        {
            double result;
            if (double.TryParse(input, out result))
            {
                return result;
            }
            return 0;
        }
        public float SafeParseToFloat(string input)
        {
            input = input.Replace(',', '.');
            CultureInfo culture = CultureInfo.InvariantCulture;
            NumberStyles style = NumberStyles.Float;

            if (float.TryParse(input, style, culture, out float result))
            {
                return result;
            }
            else
            {
                return 0f;
            }
        }
        public float ConvertIntToFloat(int value) => (float)value;
        public float ConvertBoolToFloat(bool value) => value ? 1f : 0f;
        

    }
}
