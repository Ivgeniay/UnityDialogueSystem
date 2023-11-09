using DialogueSystem.Abstract;
using System;
using UnityEngine.UIElements;

namespace DialogueSystem.TextFields
{
    public class DSTextField : TextField, IDataHolder
    {
        public Type Type { get => typeof(string); }
        public object Value { get; set; } = "";
        public bool IsFunctions { get; set; } = false;
        public string Name { get; set; } = "Text";
        public bool IsSerializedInScript { get; set; }
    }
}
