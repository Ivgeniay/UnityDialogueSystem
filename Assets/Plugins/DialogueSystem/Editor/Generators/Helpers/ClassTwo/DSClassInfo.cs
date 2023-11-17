using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Generators
{
    internal abstract class DSClassInfo
    {
        internal string ClassName = string.Empty;
        internal List<DSClassInfo> InnerClassInfo { get; private set; } = new();
        internal List<VariableInfo> VariableInfo { get; private set; } = new();
        internal List<MethodInfo> MethodInfo { get; private set; } = new();
        internal List<LambdaInfo> LambdaInfo { get; private set; } = new();

        internal abstract void Initialize();

        internal void RegisterInnerClass(DSClassInfo dSClassInfo)
        {
            if (!InnerClassInfo.Contains(dSClassInfo)) InnerClassInfo.Add(dSClassInfo);

            var t = dSClassInfo.GetType();
            var tt = GHelper.GetVarType(t);
            var count = VariableInfo.Where(el => el.Type == tt).Count();
            VariableInfo.Add(new Generators.VariableInfo()
            {
                Visibility = Visibility.@public,
                Type = tt,
                Name = dSClassInfo.ClassName + count,
            });
        }
    }


    internal class DSClassInfo<T> : DSClassInfo where T : VisualElement
    {
        internal List<IDataHolder> DataHolders { get; private set; }
        internal VisualElement instance; 

        internal DSClassInfo(VisualElement instance)
        {
            this.instance = instance;
        }
        internal override void Initialize() => DataHolders = FillIDataHolders().ToList();
        private IEnumerable<IDataHolder> FillIDataHolders()
        {
            if (instance == null) throw new NullReferenceException();

            //(Port, Node, Map) -> GrathElement -> VisualElement
            //(GrathView) -> VisualElement

            List<IDataHolder> dataHolders = new();
            switch (instance)
            {
                //Case main class
                case GraphView graphView:
                    var childrens = graphView.graphElements;
                    foreach (var child in childrens)
                        if (child is IDataHolder data) dataHolders.Add(data);
                    break;

                //Case inner class
                case GraphElement graphElement:
                    var childrens_ = graphElement.Children();
                    foreach (var child in childrens_)
                        if (child is IDataHolder data) dataHolders.Add(data);
                    break;
            }
            return dataHolders;
        }


    }
}
