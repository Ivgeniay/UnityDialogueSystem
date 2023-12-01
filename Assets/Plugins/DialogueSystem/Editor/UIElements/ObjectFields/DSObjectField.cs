using DialogueSystem.Abstract;
using DialogueSystem.Generators;
using System;
using UnityEditor.UIElements;

namespace DialogueSystem.UIElement
{
    internal class DSObjectField : ObjectField, IDataHolder
    {
        public string Name { get; set; } 
        public Type Type { get; set; } 
        public object Value { get; set; } 
        public bool IsFunctions { get; set; } 
        public bool IsSerializedInScript { get; set; }
        public Visibility Visibility { get; set; } 
        public Generators.Attribute Attribute { get; set; }

        public DSObjectField()
        {
            this.objectType = null;
        }
    }
}
