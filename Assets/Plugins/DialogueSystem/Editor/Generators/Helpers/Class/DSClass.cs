using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DialogueSystem.Generators
{
    internal class DSClass : BaseGeneratorHelper
    {
        private readonly Dictionary<BasePort, string> declaratedProperty = new();
        private readonly Dictionary<Type, VariableInfo> variable = new();


        private string classText = null;
        internal BaseNode Node { get; private set; } = null;
        internal DSClass(BaseNode parentNode)
        {
            this.Node = parentNode;
        }

        #region Class
        internal StringBuilder GetDeclaratedClass(Visibility @private, StringBuilder context, Attribute attribute = Attribute.None)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder = GetClassLine(Visibility.@private, stringBuilder, attribute);
            stringBuilder.Append(TR).Append(BR_F_OP);

            var outputs = Node.GetOutputPorts();

            foreach (var outputPort in outputs)
            {
                stringBuilder.Append(GetDeclaratedPropField(outputPort, true, Visibility.@public, Attribute.FieldSerializeField));
                stringBuilder.Append(TR);
            }

            //stringBuilder = MethodGen.Draw(context);

            stringBuilder.Append(BR_F_CL);
            classText = stringBuilder.ToString();
            context.Append(classText);

            return context;
        }
        internal string GetClassType() => DSUtilities.GenerateClassNameFromType(Node.GetType());
        private StringBuilder GetClassLine(Visibility visibility, StringBuilder context, Attribute attribute = Attribute.None)
        {
            if (attribute != Attribute.None) 
                context.Append(GetAttribute(attribute)).Append(TR);

            context
                .Append(visibility)
                .Append(SPACE)
                .Append("class")
                .Append(SPACE)
                .Append(GetClassType());
            return context;
        }
        #endregion

        #region Property
        internal string GetDeclaratedPropField(BasePort port, bool isAutoproperty = true, Visibility visibility = Visibility.@public, Attribute attribute = Attribute.None)
        {
            if (declaratedProperty.TryGetValue(port, out string prop)) return prop;
            else
            {
                var property = GenerateDeclaredProperty(port, isAutoproperty, visibility, attribute);
                declaratedProperty.Add(port, property);
                return property;
            }
        }
        private string GenerateDeclaredProperty(BasePort port, bool isAutoproperty = true, Visibility visibility = Visibility.@public, Attribute attribute = Attribute.None)
        {
            StringBuilder sb = new StringBuilder()
                        .Append(GetAttribute(attribute))
                        .Append(GetVisibility(visibility))
                        .Append(SPACE);

            var motherNode = port.node as BaseNode;
            if (motherNode is BaseOperationNode operationNode)
            {
                sb.Append("Func<")
                    .Append(GetVarType(port.portType))
                    .Append(">");
            }
            else if (motherNode is BaseConvertNode convert)
            {
                sb.Append("Func<")
                    .Append(GetVarType(port.portType))
                    .Append(">");
            }
            else sb.Append(GetVarType(port.portType));

                      sb.Append(SPACE)
                        .Append(GetVariable(port).Name);

            if (isAutoproperty)
            {
                sb.Append(SPACE)
                .Append(APROP);
            }
            return sb.ToString();
        }

        #endregion

        #region Variables
        internal VariableInfo GetVariable(BasePort port)
        {
            if (port != null)
            {
                if (variable.TryGetValue(port.GetType(), out VariableInfo vars)) return vars;
                else
                {
                    var varI = new VariableInfo();
                    var node = port.node as BaseNode;
                    varI.Name = $"{node.Model.NodeName.RemoveWhitespaces().RemoveSpecialCharacters()}_{port.portType.Name}_{variable.Count}";

                    if (node is BaseOperationNode operationNode)
                    {
                        varI.Type = port.portType;
                    }
                    else
                    {
                        varI.Type = port.portType;
                    }

                    variable.Add(port.GetType(), varI);
                    return varI;
                }
            }
            throw new NullReferenceException();
        }
        #endregion
    }

    internal class VariableInfo
    {
        public string Name;
        public Type Type;
    }
}
