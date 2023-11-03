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
        //public virtual void ChangeOutputPortType(Type type)
        //{
        //    var ports = outputContainer.Children();
        //    foreach (var port in ports)
        //    {
        //        if (port is BasePort bport)
        //        {
        //            bport.ChangeType(type);
        //            bport.ChangeName(type.Name);
        //        }
        //    }
        //}

        public override void OnConnectInputPort(BasePort _port, Edge edge)
        {
            base.OnConnectInputPort(_port, edge);

            var inpPorts = GetInputPorts();
            List<object> values = new List<object>();
            for (int i = 0; i < Model.Inputs.Count; i++) { values.Add(null); }

            foreach (BasePort port in inpPorts)
            {
                if (port.connected)
                {
                    BasePort connectedPort = port.connections.First().output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        //values.Add(connectedPort.Value);
                        int index = Model.Inputs.IndexOf(Model.Inputs.Where(e => e.PortID == port.ID).FirstOrDefault());
                        values[index] = connectedPort.Value;
                    }
                }
                if (!port.connected && port == _port)
                {
                    BasePort connectedPort = edge.output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        //values.Add(connectedPort.Value);
                        int index = Model.Inputs.IndexOf(Model.Inputs.Where(e => e.PortID == port.ID).FirstOrDefault());
                        values[index] = connectedPort.Value;
                    }
                }
            }
            if (values.Count > 0)
            {
                Do(values);
            }
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
    }
}
