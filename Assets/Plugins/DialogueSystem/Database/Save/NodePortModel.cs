using System;
using System.Collections.Generic;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class NodePortModel
    {
        public string NodeID { get; set; }
        public List<string> PortIDs { get; set; }
    }
}
