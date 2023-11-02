using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DSGroupModelSO : ScriptableObject
    {
        [SerializeField] public string ID;
        [SerializeField] public string Type;
        [SerializeField] public string GroupName;
        [SerializeField] public Vector2 Position;

        public void Init(DSGroupModel model)
        {
            ID = model.ID;
            Type = model.Type;
            GroupName = model.GroupName;
            Position = model.Position;
        }
    }
}
