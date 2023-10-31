using System;
using System.Collections.Generic;

namespace DialogueSystem.Ports
{
    public static class BasePortManager
    {
        private static List<BasePort> ports = new List<BasePort>();

        public static void UnRegister(BasePort port)
        {
            if (ports.Contains(port))
                ports.Remove(port);
        }

        public static void Register(BasePort port)
        {
            if (!ports.Contains(port))
                ports.Add(port);
        }

        public static void CallStartDrag(BasePort port)
        {
            ports.ForEach(p =>
            {
                if (p != port)
                {
                    if (HaveCommonTypes(p.AvailableTypes, port.AvailableTypes))
                    {
                        //Debug.Log($"{p.portName} is enabled");
                        p.SetEnabled(true);
                    }
                    else
                    {
                        //Debug.Log($"{p.portName} is diasabled");
                        p.SetEnabled(false);
                    }
                }
            });
        }

        public static void CallStopDrag(BasePort port)
        {
            foreach (var p in ports)
            {
                if (p != port)
                {
                    p.SetEnabled(true);
                }
            }
        }

        public static bool HaveCommonTypes(Type[] array1, Type[] array2)
        {
            foreach (var type1 in array1)
            {
                foreach (var type2 in array2)
                {
                    if (type1 == type2) return true;
                }
            }
            return false;
        }
    }
}
