using DialogueSystem.Groups;
using System.Collections.Generic;

namespace DialogueSystem.Database.Error
{
    internal class DialogueSystemGroupErrorData
    {
        internal DialogueSystemErrorData ErrorData { get; set; }
        internal List<BaseGroup> Groups { get; set; }

        public DialogueSystemGroupErrorData()
        {
            ErrorData = new();
            Groups = new();
        }
    }
}
