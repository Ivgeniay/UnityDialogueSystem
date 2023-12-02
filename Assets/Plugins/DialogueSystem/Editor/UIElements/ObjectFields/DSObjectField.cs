using DialogueSystem.Abstract;
using DialogueSystem.Generators;
using System;
using UnityEditor.UIElements;

namespace DialogueSystem.UIElement
{
    internal class DSObjectField : ObjectField, IDataHolder
    {
        public string Name { get => label; set => label = value; } 
        public Type Type { get => objectType; set => objectType = value; } 
        public object Value { get => value; set => this.value = (UnityEngine.Object)value; } 
        public bool IsFunctions { get; set; } 
        public bool IsSerializedInScript { get; set; }
        public Visibility Visibility { get; set; } 
        public Generators.Attribute Attribute { get; set; }

        public DSObjectField()
        {
            Value = null;
            //this.objectType = null;
        }
    }
}
