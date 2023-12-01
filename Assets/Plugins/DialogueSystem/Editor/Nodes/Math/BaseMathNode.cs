using UnityEditor.Experimental.GraphView;
using System.Globalization;
using DialogueSystem.Ports;
using System.Linq;
using System;
using DialogueSystem.Utilities;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseMathNode : BaseNode
    {
        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            base.OnConnectInputPort(port, edge);

            BasePort connectedPort = edge.output as BasePort;
            bool continues = BasePortManager.HaveCommonTypes(connectedPort.AvailableTypes, port.AvailableTypes);
            if (!continues) return;

            var inputPorts = GetInputPorts();

            PortInfo[] portInfos = new PortInfo[inputPorts.Count];

            for (var i = 0; i < inputPorts.Count; i++)
                portInfos[i] = new PortInfo() 
                { 
                    node = this, 
                    port = inputPorts[i], 
                    Type = inputPorts[i].Type, 
                    Value = inputPorts[i].Type.IsValueType == true ? Activator.CreateInstance(inputPorts[i].Type) : null 
                };
            

            foreach (BasePort _port in inputPorts)
            {
                if (_port.connected)
                {
                    connectedPort = _port.connections.First().output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        ChangePortValueAndType(_port, connectedPort.Type);

                        var infos = portInfos.Where(e => e.port == _port).FirstOrDefault();
                        infos.Value = connectedPort.Value;
                        infos.Type = connectedPort.Type;
                    }
                }
                else if (!_port.connected && _port != port) 
                {
                    _port.SetValue(DSUtilities.GetDefaultValue(_port.Type)); 

                    var infos = portInfos.Where(e => e.port == _port).FirstOrDefault();
                    infos.Value = _port.Value;
                    infos.Type = _port.Type;
                }
                else if (!_port.connected && _port == port)
                {
                    connectedPort = edge.output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        ChangePortValueAndType(_port, connectedPort.Type);

                        var infos = portInfos.Where(e => e.port == _port).FirstOrDefault();
                        infos.Value = connectedPort.Value;
                        infos.Type = connectedPort.Type;
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
                outputPort.SetValue(DSUtilities.GetDefaultValue(outputPort.Type));
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

            if (float.TryParse(input, style, culture, out float result)) return result;
            else return 0f;
        }
        public float ConvertIntToFloat(int value) => (float)value;
        public float ConvertBoolToFloat(bool value) => value ? 1f : 0f;
        #endregion
        
    }
}
