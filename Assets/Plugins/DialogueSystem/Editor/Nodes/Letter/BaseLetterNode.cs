using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseLetterNode : BaseNode
    {
        public object GetValue()
        {
            var output = GetOutputPorts()[0];
            return output.Value;
        }
    }
}
