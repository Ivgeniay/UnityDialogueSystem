using System.Text;
using System;

namespace DialogueSystem.Generators
{
    internal abstract class BaseGeneratorHelper
    {
        internal const string BR_F_OP = "{";
        internal const string BR_F_CL = "}";
        internal const string QUOTES = ";";
        internal const string SPACE = " ";
        internal const string L_TRIANGE = "<";
        internal const string R_TRIANGE = ">";
        internal const string TR = "\n";
        internal const string QM = "\"";
        internal const string EQLS = "=";
        internal const string NEW = "new";
        internal const string BR_OP = "(";
        internal const string BR_CL = ")";
        internal const string COMMA = ",";
        internal const string APROP = "{get; set;}";
        internal const string APROP_PRIV_GET = "{get; set;}";
        internal const string APROP_PRIV_SET = "{get; private set;}";
        internal const string APROP_PRIV = "{private get; private set;}";

        internal static string GetVisibility(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.@public:
                    return "public";
                case Visibility.@private:
                    return "private";
                case Visibility.@internal:
                    return "internal";
            }
            throw new NotImplementedException();
        }
        internal static string GetAttribute(Attribute attribute)
        {
            switch (attribute)
            {
                case Attribute.None:
                    return string.Empty;
                case Attribute.SerializeField:
                    return "[SerializeField]";
                case Attribute.FieldSerializeField:
                    return "[field: SerializeField]";
                case Attribute.SystemSerializable:
                    return "[System.Serializable]";
            }
            throw new NotImplementedException();
        }
        internal string GetVarType(Type typeVariable)
        {
            switch (typeVariable)
            {
                case Type t when t == typeof(float): return "float";
                case Type t when t == typeof(int): return "int";
                case Type t when t == typeof(string): return "string";
                case Type t when t == typeof(bool): return "bool";
                case Type t when t == typeof(decimal): return "decimal";
            }
            return typeVariable.Name;
        }

        internal virtual StringBuilder Draw(StringBuilder context) => context;

    }
}
