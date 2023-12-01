using DialogueSystem.DialogueType;
using System;
using System.Collections.Generic;
using System.Linq;

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
                        p.SetEnabled(true);
                    }
                    else
                    {
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
            if (array1.Length == 0) return false;
            if (array2.Length == 0) return false;
            if (array1.Contains(typeof(AllTypes)) || array2.Contains(typeof(AllTypes))) return true;

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
