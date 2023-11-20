using DialogueSystem.Abstract;
using System;
using System.Linq;

namespace DialogueSystem.Generators
{
    internal class VariableInfo : Info
    {
        public Visibility Visibility;
        public string Name;
        public string Type;
        public object Value;

        public override bool Equals(object obj)
        {
            if (obj is VariableInfo other)
            {
                return Visibility == other.Visibility &&
                       Name == other.Name &&
                       Type == other.Type &&
                       object.Equals(Value, other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + Visibility.GetHashCode();
                hash = (hash * 23) + (Name?.GetHashCode() ?? 0);
                hash = (hash * 23) + (Type?.GetHashCode() ?? 0);
                hash = (hash * 23) + (Value?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
