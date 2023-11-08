using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using System.Globalization;
using DialogueSystem.Ports;
using System.Linq;
using System;

namespace DialogueSystem.Nodes
{
    public abstract class BaseMathNode : BaseNode
    {
        public override void OnConnectInputPort(BasePort _port, Edge edge)
        {
            base.OnConnectInputPort(_port, edge);

            var inputPorts = GetInputPorts();

            PortInfo[] portInfos = new PortInfo[inputPorts.Count];

            for (var i = 0; i < inputPorts.Count; i++)
                portInfos[i] = new PortInfo() 
                { 
                    node = this, 
                    port = inputPorts[i], 
                    Type = inputPorts[i].portType, 
                    Value = inputPorts[i].portType.IsValueType == true ? Activator.CreateInstance(inputPorts[i].portType) : null 
                };
            

            foreach (BasePort port in inputPorts)
            {
                if (port.connected)
                {
                    BasePort connectedPort = port.connections.First().output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        ChangePort(port, connectedPort.portType);

                        var infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                        infos.Value = connectedPort.Value;
                        infos.Type = connectedPort.portType;
                    }
                }
                else if (!port.connected && port != _port) 
                {
                    port.Value = Activator.CreateInstance(port.portType);

                    var infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                    infos.Value = port.Value;
                    infos.Type = port.portType;
                }
                else if (!port.connected && port == _port)
                {
                    BasePort connectedPort = edge.output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        ChangePort(port, connectedPort.portType);

                        var infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                        infos.Value = connectedPort.Value;
                        infos.Type = connectedPort.portType;
                    }
                }
            }
            
            Do(portInfos);
        }
        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            base.OnDestroyConnectionInput(port, edge);
            var outputs = GetOutputPorts();
            foreach (BasePort outputPort in outputs)
                outputPort.Value = null;
        }


        #region Convert
        public double StringToDouble(string str)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            return 0;
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
        #endregion
        protected void ChangeOutputPortType(Type type)
        {
            var outs = GetOutputPorts();
            if (outs != null)
            {
                foreach (var outPort in outs)
                {
                    outPort.ChangeType(type);
                    outPort.ChangeName(type.Name);
                }
            }
        }
    }
}
