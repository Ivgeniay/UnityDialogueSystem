using System.Text;
using System;
using System.Reflection;
using DialogueSystem.Abstract;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace DialogueSystem.Generators
{
    internal abstract class GHelper
    {
        internal const string BR_F_OP = "{";
        internal const string BR_F_CL = "}";
        internal const string QUOTES = ";";
        internal const string COLON = ":";
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
        internal const string REG = "#region";
        internal const string ENDREG = "#endregion";
        internal const string APROP = "{get; set;}";
        internal const string APROP_PRIV_GET = "{get; set;}";
        internal const string APROP_PRIV_SET = "{get; private set;}";
        internal const string APROP_PRIV = "{private get; private set;}";

        internal static string GetVisibility(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.None: 
                    return string.Empty;
                case Visibility.@public:
                    return "public";
                case Visibility.@private:
                    return "private";
                case Visibility.@internal:
                    return "internal";
                case Visibility.@protected:
                    return "protected";
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

        internal static string GetValueWithPrefix(Type type = null, object value = null)
        {
            if (value == null) return string.Empty;

            if (type != null)
            {
                switch (type)
                {
                    case Type db when db == typeof(double): 
                        var val = value.ToString().Replace(",", ".");
                        return val.ToString() + 'd';

                    case Type fl when fl == typeof(float):
                        val = value.ToString().Replace(",", ".");
                        return val.ToString() + 'f';

                    case Type fl when fl == typeof(bool):
                        return value is true ? "true" : "false"; 

                    case Type str when str == typeof(string): return GHelper.QM + value.ToString() + GHelper.QM;
                    default: return value.ToString();
                }
            }
            else
            {
                switch (value)
                {
                    case float fl:
                        var val = value.ToString().Replace(",", ".");
                        return val.ToString() + 'f';

                    case double db:
                        val = value.ToString().Replace(",", ".");
                        return val.ToString() + 'd';

                    case string str: return GHelper.QM + value.ToString() + GHelper.QM;
                    default: return value.ToString();
                }
            }
        }

        internal static string GetVarType(Type type) => ConvertTypeToString(type);
        internal static string GetVarType<T>() => GetVarType(typeof(T));

        public static string ConvertTypeToString(Type type)
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
            typeName = typeName.Replace("+", ".");

            return typeName;
        }
        public static string RemoveSpacesAndContentBetweenParentheses(string input)
        {
            // Проверка на null или пустую строку
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder result = new StringBuilder();
            bool insideParentheses = false;
            foreach (char c in input)
            {
                if (c == '(')
                {
                    insideParentheses = true;
                }
                else if (c == ')')
                {
                    insideParentheses = false;
                }
                else if (!insideParentheses)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        result.Append(c);
                    }
                }
            }

            return result.ToString();
        }
        protected List<IDataHolder> FindAllDataHolders(VisualElement visualElement)
        {
            List<IDataHolder> dataHolders = new List<IDataHolder>();

            if (visualElement is IDataHolder holder)
                if (holder.IsSerializedInScript) dataHolders.Add(visualElement as IDataHolder);

            foreach (VisualElement childElement in visualElement.Children())
                dataHolders.AddRange(FindAllDataHolders(childElement));

            return dataHolders;
        }
    }
}
