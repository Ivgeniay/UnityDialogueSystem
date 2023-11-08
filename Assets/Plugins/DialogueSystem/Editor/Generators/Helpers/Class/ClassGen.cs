using Codice.Client.BaseCommands;
using DialogueSystem.Lambdas;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogueSystem.Generators
{
    internal class ClassGen : BaseGeneratorHelper
    {
        internal readonly Dictionary<BasePort, string> variable = new();
        private Dictionary<Type, DSClass> dsClasses = new();
        private Dictionary<BaseNode, DSClass> instances = new();

        private object[] scriptContext;
        private string className = "MyClass";

        internal PropFieldGen PropFieldGen;
        internal MethodGen MethodGen;
        internal VariablesGen VariablesGen;
        internal LambdaGenerator LambdaGenerator;

        private StringBuilder classDeclarationRegion;
        private StringBuilder propertiesRegion;
        private StringBuilder methodsRegion;
        private StringBuilder initializeMethod;

        internal ClassGen(params object[] scriptContext) 
        { 
            this.scriptContext = scriptContext;
            VariablesGen = new();
            LambdaGenerator = new(VariablesGen);
            PropFieldGen = new(VariablesGen);
            MethodGen = new(VariablesGen, PropFieldGen, LambdaGenerator, this);
        }

        internal void SetClassName(string className) => this.className = className;
        internal string GetClassName() => this.className;

        private StringBuilder GetClassLine(Visibility visibility, string className, StringBuilder context)
        {
            context
                .Append(visibility)
                .Append(SPACE)
                .Append("class")
                .Append(SPACE)
                .Append(className);
            return context;
        }
        private StringBuilder DeclarateAndInstantiateInnerClasses(StringBuilder declarationArea, StringBuilder instatiateArea)
        {
            if (scriptContext == null || scriptContext.Length == 0) return declarationArea;

            foreach (object groupedNodes in scriptContext)
            {
                if (groupedNodes is BaseNode[] grNodes)
                {
                    foreach (BaseNode node in grNodes)
                    {
                        if (dsClasses.TryGetValue(node.GetType(), out DSClass dSClass)) { }
                        else
                        {
                            declarationArea.Append(TR);
                            dSClass = GetDSClass(node);
                            dSClass.GetDeclaratedClass(Visibility.@private, declarationArea, Attribute.SystemSerializable);
                        }

                        if (instances.TryGetValue(node, out DSClass dS)) continue;
                        else
                        {
                            instances.Add(node, dSClass);
                            var prop = PropFieldGen.GetDeclaratedPropField(dSClass, node, true, Visibility.@private, Attribute.FieldSerializeField);
                            instatiateArea.AppendLine(prop);
                        }
                    }
                }
            }
            return declarationArea;
        }
        internal DSClass GetDSClass(BaseNode baseNode)
        {
            var type = baseNode.GetType();
            if (dsClasses.TryGetValue(type, out DSClass @class)) return @class;
            else
            {
                var cClass = new DSClass(baseNode);
                VariablesGen.RegisterDSClass(cClass);
                dsClasses.Add(type, cClass);
                return cClass;
            }
        }

        internal override StringBuilder Draw(StringBuilder context)
        {
            context = GetClassLine(Visibility.@public, className, context);
            context.Append(TR).Append(BR_F_OP).Append(TR);

            classDeclarationRegion = new();
            classDeclarationRegion.Append("#region ClassDeclaration").Append(TR);

            propertiesRegion = new();
            propertiesRegion.Append("#region Properties").Append(TR);

            methodsRegion = new();
            methodsRegion.Append("#region Methods").Append(TR);

            initializeMethod = new();

            DeclarateAndInstantiateInnerClasses(classDeclarationRegion, propertiesRegion);

            var initMethod = MethodGen.ConstructInitializeMethod("Initialize", instances.Keys.ToArray());
            methodsRegion.Append(initMethod);

            classDeclarationRegion.Append("#endregion").Append(TR);
            propertiesRegion.Append("#endregion").Append(TR);
            methodsRegion.Append("#endregion").Append(TR);


            context.Append(propertiesRegion);
            context.Append(methodsRegion);
            context.Append(classDeclarationRegion);

            context.Append(TR).Append(BR_F_CL);

            return context;
        }
    }
}
