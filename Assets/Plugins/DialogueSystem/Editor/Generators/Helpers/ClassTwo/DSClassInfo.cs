using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;
using DialogueSystem.Nodes;
using static PlasticGui.LaunchDiffParameters;
using Codice.CM.Common;
using DialogueSystem.Utilities;

namespace DialogueSystem.Generators
{
    internal abstract class DSClassInfo
    {
        internal protected List<IDataHolder> DataHolders { get; protected set; }
        internal string ClassName = string.Empty;
        internal ClassDrawer ClassDrawer = new();
        internal List<DSClassInfo> InnerClassInfo { get; private set; } = new();
        internal List<VariableInfo> VariableInfo { get; private set; } = new();
        internal List<MethodInfo> MethodInfo { get; private set; } = new();
        internal List<LambdaInfo> LambdaInfo { get; private set; } = new();
        internal string Type;

        internal abstract void Initialize();
        internal bool RegisterInnerClass(DSClassInfo dSClassInfo)
        {
            Type type = dSClassInfo.GetType();
            if (!InnerClassInfo.Any(e => e.GetType() == type))
            {
                InnerClassInfo.Add(dSClassInfo);
                return true;
            }
            return false;
        }
    }


    internal class DSClassInfo<T> : DSClassInfo where T : VisualElement
    {
        internal VisualElement instance;

        internal DSClassInfo(VisualElement instance)
        {
            this.instance = instance;
            Type = DSUtilities.GenerateClassNameFromType(typeof(T));
        }
        internal override void Initialize() => DataHolders = FillIDataHolders().ToList();

        //(Port, Node, Map) -> GrathElement -> VisualElement
        //(GrathView) -> VisualElement
        private IEnumerable<IDataHolder> FillIDataHolders()
        {
            if (instance == null) throw new NullReferenceException();
            List<IDataHolder> dataHolders = new();
            switch (instance)
            {
                //Case main class
                case GraphView graphView:
                    var childrens = graphView.graphElements;
                    foreach (var child in childrens) if (child is IDataHolder data) dataHolders.Add(data);
                    break;

                //Case inner class
                case GraphElement graphElement:
                    dataHolders = graphElement.GetElementsByType<IDataHolder>(predicate: (e) => e != graphElement);
                    break;
            }
            return dataHolders;
        }

        internal DSClassInfo<T> GetInnerClass<T>() where T : VisualElement => InnerClassInfo.OfType<DSClassInfo<T>>().First();
        internal DSClassInfo GetInnerClass(Type elementType) => InnerClassInfo.First(item => item.GetType().IsAssignableFrom(elementType));
        internal DSClassInfo GetInnerClass(string elementType) => InnerClassInfo.First(item => item.Type == elementType);


    }
}
