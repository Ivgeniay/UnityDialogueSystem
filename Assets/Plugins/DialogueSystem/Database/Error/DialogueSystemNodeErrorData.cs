using DialogueSystem.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueSystem.Database.Error
{
    internal class DialogueSystemNodeErrorData
    {
        internal DialogueSystemErrorData errorData { get; set; }
        internal List<BaseNode> Nodes { get; set; }

        internal DialogueSystemNodeErrorData()
        {
            errorData = new();
            Nodes = new List<BaseNode>();
        }
    }
}
