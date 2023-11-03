using DialogueSystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DialogueSystem.Generators
{
    internal static class GeneratorUtility
    {
        internal const string BRACKET_CLOSE = "}\n";
        internal const string BRACKET_OPEN = "{\n";
        internal const string QUOTES = ";\n";
        internal const string SPACE = " ";
        internal const string TR = "\n";

        private static readonly Dictionary<StringNode, string> letterVariables = new(); 
        private static readonly Dictionary<ActorNode, string> actorVariables = new(); 
        private static readonly Dictionary<IntegerNode, string> intVariables = new();
        private static readonly Dictionary<FloatNode, string> floaVariables = new();

        internal static string GetUsings(params object[][] types)
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


        internal static string GetNamespaces(object[] objects)
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

        internal static string GetStringVariable(StringNode node)
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
        internal static string GetIntVariable(IntegerNode node) 
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
        internal static string GetFloatVariable(FloatNode node)
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
        internal static string GetActorVariable(ActorNode node)
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

        internal static string GeneratePropery(BaseNode node, Visibility visibility = Visibility.Public)
        {
            string variable = GetVisibility(visibility);
            switch (node)
            {
                case IntegerNode i:
                    variable += SPACE + GetVarType(TypeVariable.Int) + SPACE;
                    variable += GetIntVariable(i) + SPACE + GetAutoProperty();
                    variable += SPACE + "=" + $" {i.GetValue()}" + QUOTES;
                    break;
                case FloatNode f:
                    variable += SPACE + GetVarType(TypeVariable.Float) + SPACE;
                    variable += GetFloatVariable(f) + SPACE + GetAutoProperty();
                    variable += SPACE + "=" + $" {f.GetValue()}" + "f" + QUOTES;
                    break;
                case StringNode s:
                    variable += SPACE + GetVarType(TypeVariable.String) + SPACE;
                    variable += GetStringVariable(s) + " " + GetAutoProperty();
                    variable += SPACE + "=" + $" \"{s.GetValue()}\"" + QUOTES;
                    break;
                case ActorNode a:
                    variable += SPACE + a.ActorType.Name + SPACE;
                    variable += GetActorVariable(a) + " " + GetAutoProperty();
                    break;

                default: 
                    throw new NotImplementedException();
            }
            return variable + "\n";
        }

        internal static string GetClassLine(string className) => $"public class {className}\n";
        internal static string GetVisibility(Visibility visibility)
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
        internal static string GetInternal() => "internal";
        internal static string GetPrivate() => "private";
        internal static string GetVarType(TypeVariable typeVariable)
        {
            switch (typeVariable)
            {
                case TypeVariable.Int:return "int";
                case TypeVariable.Float: return "float";
                case TypeVariable.String: return "string";
                case TypeVariable.Bool: return "bool";
                case TypeVariable.Decimal: return "decimal";
            }
            throw new NotImplementedException();
        }
        internal static string GetAutoProperty() => "{ get; set; }";
    }

    public enum Visibility
    {
        Public,
        Internal,
        Private
    }

    public enum TypeVariable
    {
        Int,
        Float,
        String,
        Bool,
        Decimal,
    }
}
