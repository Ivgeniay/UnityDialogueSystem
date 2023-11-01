using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DSGroupSO : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }

        public void Init(string groupName) => GroupName = groupName;
    }
}
