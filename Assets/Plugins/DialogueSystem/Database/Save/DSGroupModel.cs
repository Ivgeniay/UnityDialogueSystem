using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSGroupModel
    {
        [SerializeField] public string ID;
        [SerializeField] public string Type;
        [SerializeField] public string GroupName;
        [SerializeField] public Vector2 Position;
    }
}
