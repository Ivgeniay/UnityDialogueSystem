using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DialogueSystemGroupSO : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }

        public void Init(string groupName) => GroupName = groupName;
    }
}
