using System.Text;
using System;
using System.Reflection;

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
        internal const string THIS = "this.";
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

        internal string GetVarType(Type type)
        {
            return ConvertTypeToString(type);
        }

        internal string GetVarType<T>() => GetVarType(typeof(T));

        public string ConvertTypeToString(Type type)
        {
            string typeName = type.ToString();
            int backtickIndex = typeName.IndexOf('`');
            if (backtickIndex >= 0)
            {
                typeName = typeName.Remove(backtickIndex, 2);
                typeName += "<";

                Type[] genericArguments = type.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    typeName += i > 0 ? ", " : "";
                    typeName += ConvertTypeToString(genericArguments[i]);
                }

                typeName += ">";
            }
            if (typeName.IndexOf("[") != -1)
            {
                var startIndex = typeName.IndexOf("[");
                var finishIndex = typeName.IndexOf("]") + 1;
                typeName = typeName.Remove(startIndex, finishIndex - startIndex);
            }
            return typeName;
        }

        internal virtual StringBuilder Draw(StringBuilder context) => context;

    }
}
