using System.Collections.Generic;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using System.Text;
using System;

namespace DialogueSystem.Generators
{
    internal class PropFieldGen : BaseGeneratorHelper
    {
        private List<string> propertyToDraw = new List<string>();
        private VariablesGen variablesGen;
        internal PropFieldGen(VariablesGen variablesGen) 
        {
            this.variablesGen = variablesGen;
        }

        internal void GeneratePropField(BaseNode node, bool isAutoproperty = true, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            var outputs = node.GetOutputPorts();
            foreach (var outputPort in outputs )
            {
                StringBuilder sb = new StringBuilder()
                    .Append(GetAttribute(attribute))
                    .Append(GetVisibility(visibility))
                    .Append(SPACE)
                    .Append(GetVarType(outputPort.portType))
                    .Append(SPACE)
                    .Append(variablesGen.GetVariable(outputPort));

                if (isAutoproperty)
                {
                    sb.Append(SPACE)
                    .Append(APROP);
                }

                if (outputPort.Value != null)
                {
                    sb.Append(SPACE).Append(EQLS).Append(SPACE);
                    if (outputPort.portType == typeof(string)) sb.Append(QM);
                    if (outputPort.portType == typeof(float)) sb.Append(outputPort.Value.ToString().Replace(',', '.')).Append("f");
                    else if (outputPort.portType == typeof(double)) sb.Append(outputPort.Value.ToString().Replace(',', '.')).Append("d");
                    else sb.Append(outputPort.Value);
                    if (outputPort.portType == typeof(string)) sb.Append(QM);
                    sb.Append(QUOTES);
                }
                propertyToDraw.Add(sb.ToString());
            }
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

        internal override StringBuilder Draw(StringBuilder context)
        {
            foreach (var prop in propertyToDraw)
                context.AppendLine(prop);
            
            return context;
        }
    }
}
