﻿using DialogueSystem.Database.Save;
using UnityEditor.Experimental.GraphView;
using DialogueSystem.Utilities;
using DialogueSystem.Ports;

namespace DialogueSystem.Nodes
{
    public abstract class BaseNumbersNode : BaseMathNode
    {
        public object GetValue()
        {
            var output = GetOutputPorts()[0];
            return output.Value;
        }
    }
}
