using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using System;

namespace DialogueSystem.UIElement
{
    internal class DSDropdownField : DropdownField//, IDataHolder
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public bool IsFunctions { get; set; }
        public bool IsSerializedInScript { get; set; }
        public Generators.Visibility Visibility { get; set; }
        public Generators.Attribute Attribute { get; set; }
    }
}
