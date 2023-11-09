using System;

namespace DialogueSystem.Abstract
{
    //Interface - mark for VisualElement whitch holddata to generate script
    internal interface IDataHolder
    {
        public string Name { get; }
        public Type Type { get; }
        public object Value { get; }
        public bool IsFunctions { get; }
        public bool IsSerializedInScript { get; }
    }
}
