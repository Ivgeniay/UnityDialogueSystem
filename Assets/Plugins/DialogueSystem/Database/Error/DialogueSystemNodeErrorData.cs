using DialogueSystem.Nodes;
using System.Collections.Generic;

namespace DialogueSystem.Database.Error
{
    internal class DialogueSystemNodeErrorData
    {
        internal DialogueSystemErrorData ErrorData { get; set; }
        internal List<BaseNode> Nodes { get; set; }

        internal DialogueSystemNodeErrorData()
        {
            ErrorData = new();
            Nodes = new List<BaseNode>();
        }
    }
}
