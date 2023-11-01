using DialogueSystem.Database.Save;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Save
{
    public class GraphSO : ScriptableObject
    {
        [field: SerializeField] public string FileName {  get; set; }
        [field: SerializeField] public List<DSNodeModel> NodeModels { get; set; }
        [field: SerializeField] public List<DSGroupModel> GroupModels { get; set; }

        public void Init(string fileName)
        {
            FileName = fileName;
            NodeModels = new List<DSNodeModel>();
            GroupModels = new List<DSGroupModel>();
        }
    }
}
