using DialogueSystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DialogueSystem.Generators
{
    internal static class GU
    {
        internal const string BRACKET_CLOSE = "}\n";
        internal const string BRACKET_OPEN = "{\n";
        internal const string QUOTES = ";\n";
        internal const string SPACE = " ";
        internal const string TR = "\n";

        private static ClassD classD;
        private static Namesp namesp;
        private static Vars vars;
        static GU()
        {
            classD = new();
            namesp = new();
            vars = new();
        }

        #region Usings
        internal static string GetUsings(params object[][] types) => namesp.GetUsings(types);
        #endregion

        #region Variables
        internal static string GetStringVariable(StringNode node) => vars.GetStringVariable(node);
        internal static string GetIntVariable(IntegerNode node) => vars.GetIntVariable(node);
        internal static string GetFloatVariable(FloatNode node) => vars.GetFloatVariable(node);
        internal static string GetActorVariable(ActorNode node) => vars.GetActorVariable(node);
        #endregion

        #region Propertyes
        internal static string GeneratePropery(BaseNode node, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None) => vars.GeneratePropery(node, visibility, attribute);
        #endregion

        internal static string GetClassLine(string className) => classD.GetClassLine(className);
    }

    public class ClassD
    {
        internal string GetClassLine(string className) => $"public class {className}\n";
    }
    public class Namesp
    {
        internal string GetUsings(params object[][] types)
        {
            StringBuilder sb = new StringBuilder();
            List<string> strings = new List<string>();

            foreach (var type in types)
            {
                var st = GetNamespaces(type);
                if (st != null && !strings.Contains(st))
                    strings.Add(st);
            }

            foreach (var t in strings) sb.Append(t);
            return sb.ToString();
        }
        internal string GetNamespaces(object[] objects)
        {
            StringBuilder sb = new StringBuilder();
            List<string> strings = new List<string>();

            foreach (object t in objects)
            {
                if (!strings.Contains($"using {t.GetType().Namespace};\n"))
                    strings.Add($"using {t.GetType().Namespace};\n");
            }

            foreach (string s in strings)
                sb.Append(s);

            return sb.ToString();
        }
    }
    public class Vars
    {
        private readonly Dictionary<StringNode, string> letterVariables = new();
        private readonly Dictionary<ActorNode, string> actorVariables = new();
        private readonly Dictionary<IntegerNode, string> intVariables = new();
        private readonly Dictionary<FloatNode, string> floaVariables = new();

        internal string GetStringVariable(StringNode node)
        {
            if (node != null)
            {
                if (letterVariables.TryGetValue(node, out string variable))
                {
                    return variable;
                }
                else
                {
                    variable = $"strV{letterVariables.Count}";
                    letterVariables.Add(node, variable);
                    return variable;
                }
            }
            throw new NullReferenceException();
        }
        internal string GetIntVariable(IntegerNode node)
        {
            if (node != null)
            {
                if (intVariables.TryGetValue(node, out string variable))
                {
                    return variable;
                }
                else
                {
                    variable = $"intV{intVariables.Count}";
                    intVariables.Add(node, variable);
                    return variable;
                }
            }
            throw new NullReferenceException();
        }
        internal string GetFloatVariable(FloatNode node)
        {
            if (node != null)
            {
                if (floaVariables.TryGetValue(node, out string variable))
                {
                    return variable;
                }
                else
                {
                    variable = $"intV{floaVariables.Count}";
                    floaVariables.Add(node, variable);
                    return variable;
                }
            }
            throw new NullReferenceException();
        }
        internal string GetActorVariable(ActorNode node)
        {
            if (node != null)
            {
                if (actorVariables.TryGetValue(node, out string variable))
                {
                    return variable;
                }
                else
                {
                    variable = $"actorV{actorVariables.Count}";
                    actorVariables.Add(node, variable);
                    return variable;
                }
            }
            throw new NullReferenceException();
        }

        internal string GeneratePropery(BaseNode node, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            string variable = GetAttribute(attribute) + GetVisibility(visibility);

            switch (node)
            {
                case IntegerNode i:
                    variable += GU.SPACE + GetVarType(TypeVariable.Int) + GU.SPACE;
                    variable += GetIntVariable(i) + GU.SPACE + GetAutoProperty();
                    variable += GU.SPACE + "=" + $" {i.GetValue()}" + GU.QUOTES;
                    break;
                case FloatNode f:
                    variable += GU.SPACE + GetVarType(TypeVariable.Float) + GU.SPACE;
                    variable += GetFloatVariable(f) + GU.SPACE + GetAutoProperty();
                    variable += GU.SPACE + "=" + $" {f.GetValue()}" + "f" + GU.QUOTES;
                    break;
                case StringNode s:
                    variable += GU.SPACE + GetVarType(TypeVariable.String) + GU.SPACE;
                    variable += GetStringVariable(s) + " " + GetAutoProperty();
                    variable += GU.SPACE + "=" + $" \"{s.GetValue()}\"" + GU.QUOTES;
                    break;
                case ActorNode a:
                    variable += GU.SPACE + a.ActorType.Name + GU.SPACE;
                    variable += GetActorVariable(a) + " " + GetAutoProperty();
                    break;

                default:
                    throw new NotImplementedException();
            }
            return variable + "\n";
        }

        internal string GetClassLine(string className) => $"public class {className}\n";
        internal string GetVisibility(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Public:
                    return " public";
                case Visibility.Private:
                    return " private";
                case Visibility.Internal:
                    return " internal";
            }
            throw new NotImplementedException();
        }
        internal string GetInternal() => "internal";
        internal string GetPrivate() => "private";
        internal string GetVarType(TypeVariable typeVariable)
        {
            switch (typeVariable)
            {
                case TypeVariable.Int: return "int";
                case TypeVariable.Float: return "float";
                case TypeVariable.String: return "string";
                case TypeVariable.Bool: return "bool";
                case TypeVariable.Decimal: return "decimal";
            }
            throw new NotImplementedException();
        }
        internal string GetAutoProperty() => "{ get; set; }";
        internal string GetAttribute(Attribute attribute)
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
    }


    internal enum Visibility
    {
        Public,
        Internal,
        Private
    }
    internal enum TypeVariable
    {
        Int,
        Float,
        String,
        Bool,
        Decimal,
    }
    internal enum Attribute
    {
        None,
        SerializeField,
        FieldSerializeField,
    }
}
