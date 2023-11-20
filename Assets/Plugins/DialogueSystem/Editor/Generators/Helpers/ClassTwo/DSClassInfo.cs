using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;
using DialogueSystem.Utilities;

namespace DialogueSystem.Generators
{
    internal abstract class DSClassInfo
    {
        internal IDataHolder IDataHolder { get; set; }
        internal protected List<IDataHolder> DataHolders { get; protected set; }
        internal string ClassName = string.Empty;
        internal ClassDrawer ClassDrawer = new();

        internal List<DSClassInfo> InnerClassInfo { get; private set; } = new();
        internal List<VariableInfo> VariableInfo { get; private set; } = new();
        internal List<MethodInfo> MethodInfo { get; private set; } = new();
        internal List<LambdaInfo> LambdaInfo { get; private set; } = new();
        internal string Type;


        internal abstract void Initialize();
        

        internal bool RegisterInnerClassDeclaration(DSClassInfo dSClassInfo)
        {
            if (!InnerClassInfo.Any(e => e.Type == dSClassInfo.Type))
            {
                InnerClassInfo.Add(dSClassInfo);
                return true;
            }
            return false;
        }
        internal VariableInfo GetVariable(IDataHolder dataHolder) => VariableInfo.FirstOrDefault(e => e.DataHolder == dataHolder);
        

        internal DSClassInfo GetInnerDSClass(DSClassInfo dsClass)
        {
            VariableInfo varInfo = VariableInfo.FirstOrDefault(e => e.ClassInfo.Equals(dsClass));
            if (varInfo == null) return null;
            return varInfo.ClassInfo;
        }

        internal DSClassInfo GetInnerDSClass(IDataHolder dataHolder)
        {
            VariableInfo varInfo = VariableInfo.FirstOrDefault(e => e.DataHolder == dataHolder);
            if (varInfo != null) return varInfo.ClassInfo;
            return null;
        }
        internal void SetType(string type) => Type = type;
        internal void AddTypePefix(string prefix) => Type += prefix;

        public override bool Equals(object obj)
        {
            if (obj is DSClassInfo other)
            {
                return ClassName == other.ClassName &&
                       //Type == other.Type &&
                       InnerClassInfo.SequenceEqual(other.InnerClassInfo) &&
                       VariableInfo.SequenceEqual(other.VariableInfo);
            }
            return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + ClassName?.GetHashCode() ?? 0;
                //hash = (hash * 23) + Type?.GetHashCode() ?? 0;

                foreach (var innerClassInfo in InnerClassInfo) hash = (hash * 23) + innerClassInfo?.GetHashCode() ?? 0;
                foreach (var variableInfo in VariableInfo) hash = (hash * 23) + variableInfo?.GetHashCode() ?? 0;
                return hash;
            }
        }
    }


    internal class DSClassInfo<T> : DSClassInfo where T : VisualElement
    {
        internal VisualElement instance;

        internal DSClassInfo(VisualElement instance)
        {
            this.instance = instance;
            Type = DSUtilities.GenerateClassNameFromType(typeof(T));

            Initialize();

            foreach (var data in DataHolders)
                Type += "_" + DSUtilities.GenerateClassPefixFromType(data.Type);
            
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



    }
}
