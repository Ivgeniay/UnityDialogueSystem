using DialogueSystem.Abstract;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using DialogueSystem.TextFields;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Generators
{
    internal class DSClass : BaseGeneratorHelper
    {
        private readonly Dictionary<IDataHolder, string> declaratedProperty = new();
        private readonly Dictionary<IDataHolder, VariableInfo> variable = new();
        private List<IDataHolder> dataHolders = new List<IDataHolder>();
        private string classText { get; set; } = null;
        internal VisualElement parentVisualElement { get; private set; } = null;

        internal void Initialize(VisualElement visualElement)
        {
            parentVisualElement = visualElement;
            dataHolders = FindAllDataHolders(visualElement);
        }
        
        #region Class
        internal StringBuilder GetDeclaratedClass(Visibility @private, StringBuilder context, Attribute attribute = Attribute.None)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder = GetClassLine(Visibility.@private, stringBuilder, attribute);
            stringBuilder.Append(TR).Append(BR_F_OP);

            foreach (var dataHolder in dataHolders)
            {
                stringBuilder.Append(GetDeclaratedPropField(dataHolder, true, Visibility.@public, Attribute.FieldSerializeField));
                stringBuilder.Append(TR);
            }

            stringBuilder.Append(BR_F_CL);
            classText = stringBuilder.ToString();
            context.Append(classText);

            return context;
        }
        internal string GetClassType()
        {
            if (parentVisualElement is ActorNode actor) return actor.ActorType.ToString();
            return DSUtilities.GenerateClassNameFromType(parentVisualElement.GetType());
        }
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
        internal string GetDeclaratedPropField(IDataHolder dataH, bool isAutoproperty = true, Visibility visibility = Visibility.@public, Attribute attribute = Attribute.None)
        {
            if (declaratedProperty.TryGetValue(dataH, out string prop)) return prop;
            else
            {
                if (dataH is IDataHolder dataHolder)
                {
                    var property = GenerateDeclaredPortProperty(dataHolder, isAutoproperty, visibility, attribute);
                    declaratedProperty.Add(dataHolder, property);
                    return property;
                }

                throw new NotImplementedException();
            }
        }
        private string GenerateDeclaredPortProperty(IDataHolder dataHolder, bool isAutoproperty = true, Visibility visibility = Visibility.@public, Attribute attribute = Attribute.None)
        {
            StringBuilder sb = new StringBuilder()
            .Append(GetAttribute(attribute))
            .Append(GetVisibility(visibility))
            .Append(SPACE);

            if (dataHolder.IsFunctions)
            {
                sb.Append("Func<")
                    .Append(GetVarType(dataHolder.Type))
                    .Append(">");
            }
            else sb.Append(GetVarType(dataHolder.Type));
            
            sb.Append(SPACE).Append(GetVariable(dataHolder).Name);

            if (isAutoproperty)
            {
                sb.Append(SPACE)
                .Append(APROP);
            }
            return sb.ToString();
        }
        #endregion
        #region Variables
        internal VariableInfo GetVariable(IDataHolder dataHolder)
        {
            if (variable.TryGetValue(dataHolder, out VariableInfo vars)) return vars;
            else
            {
                var varI = new VariableInfo();
                BaseNode node = null;

                VisualElement currentElement = dataHolder as VisualElement;
                while (currentElement != null)
                {
                    if (currentElement is BaseNode _node)
                    {
                        node = _node;
                        break;
                    }
                    currentElement = currentElement.parent;
                }

                if (node is ActorNode actorNode)
                {
                    varI.Name = $"{dataHolder.Name.Substring(0, dataHolder.Name.IndexOf(" "))}";
                }
                else
                {
                    var t = dataHolder.GetType();
                    var count = variable.Where(e => e.Key.GetType().IsAssignableFrom(t)).Count();
                    varI.Name = $"{node.Model.NodeName.RemoveWhitespaces().RemoveSpecialCharacters()}_{dataHolder.Type.Name}_{count}";
                }
                varI.Type = dataHolder.Type;

                variable.Add(dataHolder, varI);
                return varI;
            }
        }
        #endregion
        private List<IDataHolder> FindAllDataHolders(VisualElement visualElement)
        {
            List<IDataHolder> dataHolders = new List<IDataHolder>();

            if (visualElement is IDataHolder holder)
                if (holder.IsSerializedInScript) dataHolders.Add(visualElement as IDataHolder);

            foreach (VisualElement childElement in visualElement.Children())
                dataHolders.AddRange(FindAllDataHolders(childElement));

            return dataHolders;
        }
    }

    internal class VariableInfo
    {
        public string Name;
        public Type Type;
    }
}
