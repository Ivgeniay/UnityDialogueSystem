using System.Collections.Generic;
using DialogueSystem.Nodes;
using DialogueSystem.Text;
using System.Reflection;
using System.Text;
using System;

namespace DialogueSystem.Generators
{
    internal static class GU
    {
        internal const string BRACKET_CLOSE = "}\n";
        internal const string BRACKET_OPEN = "{\n";
        internal const string QUOTES = ";\n";
        internal const string SPACE = " ";
        internal const string TR = "\n";

    }

    internal class ClassD
    {
        private ClassC c;
        public ClassD() => c = new();
        


        internal string GetClassLine(string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public class ")
                .Append(className)
                .Append("\n")
                .Append("{")
                .Append("\n")
                .Append("}");
            return sb.ToString();
        }
        private class ClassC
        {
            private ClassB b;
            public ClassC() => b = new();

            private string ClassLine;
            private string Property;
            private string Fields;
            private string Methods;

            internal class ClassB
            {
                public string DrawClass()
                {
                    return "";
                }
            }
        }
    }

    internal class Namesp
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
                var names = t.GetType().Namespace;
                if (names == "System")
                {
                    string assemblyFullName = "Assembly-CSharp";
                    Assembly assembly = Assembly.Load(assemblyFullName);
                    Type ty = assembly.GetType(t.ToString());
                    names = ty.Namespace;

                    if (!strings.Contains($"using {names};\n"))
                        strings.Add($"using {names};\n");
                }
                else
                {
                    if (!strings.Contains($"using {t.GetType().Namespace};\n"))
                        strings.Add($"using {t.GetType().Namespace};\n");
                }

            }

            foreach (string s in strings)
                sb.Append(s);

            return sb.ToString();
        }
    }
    internal class Vars
    {
        private readonly Dictionary<Type, Dictionary<BaseNode, string>> variables = new();

        internal string GetVariable(BaseNode node)
        {
            if (node != null)
            {
                if (variables.TryGetValue(node.GetType(), out Dictionary<BaseNode, string> vars))
                {
                    if (vars.TryGetValue(node, out string variableName)) return variableName;
                    else
                    {
                        variableName = $"{node.Model.NodeName.RemoveWhitespaces().RemoveSpecialCharacters()}_{vars.Count}";
                        vars.Add(node, variableName);
                        return variableName;
                    }
                }
                else
                {
                    var dic = new Dictionary<BaseNode, string>();
                    var variableName = $"{node.Model.NodeName.RemoveWhitespaces().RemoveSpecialCharacters()}_{dic.Count}";
                    dic.Add(node, variableName);
                    variables.Add(node.GetType(), dic);
                    return variableName;
                }
            }
            throw new NullReferenceException();
        }
        internal string GeneratePropery(BaseNode node, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            StringBuilder sb = new StringBuilder()
                //.Append(GU.GetAttribute(attribute))
                //.Append(GU.GetVisibility(visibility))
                .Append(GU.SPACE);

            switch (node)
            {
                case IntegerNode i:
                    sb
                        .Append(GetVarType(TypeVariable.Int))
                        .Append(GU.SPACE)
                        .Append(GetVariable(i))
                        .Append(GU.SPACE)
                        .Append(GetAutoProperty());
                    
                    if ((int)i.GetValue() != 0)
                    {
                        sb
                            .Append(GU.SPACE)
                            .Append("=")
                            .Append(i.GetValue())
                            .Append(GU.QUOTES);
                    }
                    break;
                case FloatNode f:
                    sb
                        .Append(GetVarType(TypeVariable.Float))
                        .Append(GU.SPACE)
                        .Append(GetVariable(f))
                        .Append(GU.SPACE)
                        .Append(GetAutoProperty());

                    if ((float)f.GetValue() != 0)
                    {
                        sb.Append(GU.SPACE)
                            .Append("=")
                            .Append(GU.SPACE)
                            .Append(f.GetValue().ToString().Replace(',', '.'))
                            .Append("f")
                            .Append(GU.QUOTES);
                    }
                    break;
                case StringNode s:
                    sb
                        .Append(GetVarType(TypeVariable.String))
                        .Append(GU.SPACE)
                        .Append(GetVariable(s))
                        .Append(GU.SPACE)
                        .Append(GetAutoProperty());

                    if ((string)s.GetValue() != "")
                    {
                        sb.Append(GU.SPACE)
                            .Append("=")
                            .Append(GU.SPACE)
                            .Append("\"")
                            .Append(s.GetValue())
                            .Append("\"")
                            .Append(GU.QUOTES);
                    }
                    break;
                case ActorNode a:
                    sb
                        .Append(a.ActorType.Name)
                        .Append(GU.SPACE)
                        .Append(GetVariable(a))
                        .Append(GU.SPACE)
                        .Append(GetAutoProperty());
                    break;

                default:
                    throw new NotImplementedException();
            }
            return sb.Append(GU.TR).ToString();
        }


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
        
    }
    internal class Methods
    {
        private Dictionary<BaseNode, MethodInfo> methodInfos { get; set; } = new();
        private Dictionary<BaseNode, string> methods { get; set; } = new();
        private Vars vars;
        internal Methods(Vars vars) 
        {
            this.vars = vars;
        }

        internal string GetCallMethod(BaseNode node, params object[] param)
        {
            if (methodInfos.TryGetValue(node, out MethodInfo methodName))
            {
                StringBuilder sb = new();
                return methodInfos + "();";
            }
            else
            {
                GetMethod(node);
                return GetCallMethod(node);
            }
        }
        internal string GetMethod(BaseNode node, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            if (methods.TryGetValue(node, out string methodStr)) return methodStr;
            else
            {
                StringBuilder sb = new StringBuilder()
                    //.Append(GU.GetAttribute(attribute))
                    //.Append(GU.GetVisibility(visibility))
                    .Append(GU.SPACE);
                    MethodInfo methodInfo = GetMethodInfo(node);
                    if (methodInfo.CountOutParams == 0) sb.Append("void").Append(GU.SPACE);
                    else if (methodInfo.CountOutParams == 1) sb.Append(methodInfo.OutputParamTypes[0].Name).Append(GU.SPACE);
                    else
                    {
                        sb.Append("(");
                        for (int i = 0; i < methodInfo.CountOutParams; i++)
                        {
                            sb.Append(methodInfo.OutputParamTypes[i].Name);
                            if (i != methodInfo.CountOutParams) sb.Append(", ");
                        }
                        sb.Append(")");
                    }
                    sb  .Append(GU.SPACE)
                        .Append(node.Model.NodeName)
                        .Append("(");

                switch (node)
                {
                    case AdditionNode add:
                        if (methodInfo.CountParams > 0)
                        {
                            string vName = "item_";
                            int count = 0;
                            for (int i = 0;i < methodInfo.CountParams;i++)
                            {
                                sb  .Append(methodInfo.InputParamTypes[i].Name)
                                    .Append(GU.SPACE)
                                    .Append(vName)
                                    .Append(count);

                                if (i != methodInfo.CountParams - 1)
                                {
                                    sb  .Append(',')
                                        .Append(GU.SPACE);
                                }
                                count++;
                            }
                        }
                        sb.Append(")");
                        break;
                }

                sb.Append("\n")
                    .Append("{")
                    .Append("\n")
                    .Append("}")
                    .Append("\n");

                return sb.ToString();
            }
        }


        internal string CallMethod(BaseNode node, params BaseNode[] methodParams)
        {
            StringBuilder sb = new StringBuilder();
            var methodInfo = GetMethodInfo(node);

            sb
                .Append(methodInfo.MethodName)
                .Append('(');

            for (int i = 0; i < methodParams.Length; i++)
            {
                var variable = vars.GetVariable(methodParams[i]);
                sb.Append(variable);
                    if(i != methodParams.Length - 1) sb.Append(", ");
            }

            sb.Append(");\n");
            return sb.ToString();
        }

        private MethodInfo GenerateMethodInfo(BaseNode baseNode, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            var inputs = baseNode.GetInputPorts();
            var outputs = baseNode.GetOutputPorts();

            List<Type> inputParamTypes = new();
            List<Type> outputParamTypes = new();

            foreach (var port in inputs)
                inputParamTypes.Add(port.portType);
            foreach (var port in outputs)
                outputParamTypes.Add(port.portType);

            return new MethodInfo()
            {
                InputParamTypes = inputParamTypes.ToArray(),
                OutputParamTypes = outputParamTypes.ToArray(),
                MethodName = baseNode.Model.NodeName,
                Visibility = visibility,
                Attribute = attribute,
                CountParams = inputParamTypes.Count,
                CountOutParams = outputParamTypes.Count
            };
        }
        private MethodInfo GetMethodInfo(BaseNode node)
        {
            if (methodInfos.TryGetValue(node, out MethodInfo method)) return method;
            else return GenerateMethodInfo(node);
        }

        private record MethodInfo
        {
            public int CountParams;
            public int CountOutParams;
            public Type[] InputParamTypes;
            public Type[] OutputParamTypes;
            public string MethodName;
            public Visibility Visibility;
            public Attribute Attribute;
            public string OutputParamT;
        }
    }

    public class NodeModel
    {
        public string VariableName { get; set; }
        public Type[] InputType { get; set; }
        public Type[] OutputType { get; set; }
    }
}
