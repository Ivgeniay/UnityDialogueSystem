using DialogueSystem.Groups;
using System.Collections.Generic;

namespace DialogueSystem.Database.Error
{
    internal class DSGroupErrorData
    {
        internal DSErrorData ErrorData { get; set; }
        internal List<BaseGroup> Groups { get; set; }

        public DSGroupErrorData()
        {
            ErrorData = new();
            Groups = new();
        }
    }
}
