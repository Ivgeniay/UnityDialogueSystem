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
        internal const string TR = "\n";
        internal const string QM = "\"";
        internal const string EQLS = "=";
        internal const string BR_OP = "(";
        internal const string BR_CL = ")";
        internal const string APROP = "{get; set;}";
        internal const string APROP_PRIV_GET = "{get; set;}";
        internal const string APROP_PRIV_SET = "{get; private set;}";
        internal const string APROP_PRIV = "{private get; private set;}";

        internal static string GetVisibility(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Public:
                    return "public";
                case Visibility.Private:
                    return "private";
                case Visibility.Internal:
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
            }
            throw new NotImplementedException();
        }

        internal virtual StringBuilder Draw(StringBuilder context) => context;

    }
}
