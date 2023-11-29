using DialogueSystem.Abstract;
using DialogueSystem.Nodes;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using System.Reflection;
using System.Linq;
using System.Text;
using System;
using static DialogueSystem.DialogueDisposer;
using static DialogueSystem.DialogueDisposer.DSDialogueOption;

namespace DialogueSystem.Generators
{
    internal class ClassGenerator : GHelper
    {
        private DSClassInfo<DSGraphView> dsGrathViewClass = null;
        private DSGraphView graphView = null;

        internal ClassGenerator(DSGraphView grathView, string className)
        {
            this.graphView = grathView;
            dsGrathViewClass = new(grathView);
            dsGrathViewClass.ClassName = className;
        }

        internal void Initialize()
        {
            foreach (IDataHolder dataHolder in dsGrathViewClass.DataHolders)
                Initialize(dataHolder, dsGrathViewClass);
        }


        private DSClassInfo Initialize(IDataHolder dataHolder, DSClassInfo<DSGraphView> intoDSClassInfo)
        {
            DSClassInfo innerDsClass = intoDSClassInfo.GetInnerDSClass(dataHolder);
            if (innerDsClass != null) return innerDsClass;
            VariableInfo mainVarInfo = null;

            switch (dataHolder)
            {
                case BasePrimitiveNode primitive:
                    innerDsClass = CreateInnerClass(dataHolder);
                    mainVarInfo = AddDSClassToMain(innerDsClass, intoDSClassInfo);

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                    for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                    {
                        string type = innerDsClass.VariableInfo[i].Type;
                        dsGrathViewClass.ClassDrawer.AddInitializeObject
                            (
                                mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                returnRefType: null,
                                paramType: Type.GetType(type),
                                innerDsClass.VariableInfo[i].Value
                            );
                    }
                    break;

                case BaseOperationNode operation:
                    innerDsClass = CreateInnerClass(dataHolder);
                    mainVarInfo = AddDSClassToMain(innerDsClass, intoDSClassInfo);

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                    for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                    {
                        string type = innerDsClass.VariableInfo[i].Type;
                        BasePort port = innerDsClass.VariableInfo[i].DataHolder as BasePort;
                        BaseNode node = port.node as BaseNode;

                        if (port.PortSide == PortSide.Input)
                        {
                            if (port.connected)
                            {
                                BasePort outputPort = port.connections.ToList()[0].output as BasePort;
                                BaseNode outputNode = outputPort.node as BaseNode;
                                DSClassInfo outputDsClass = Initialize(outputNode, intoDSClassInfo);
                                VariableInfo outputPortVarInfo = outputDsClass.GetVariable(outputPort);
                                VariableInfo outputNodeVarInfo = intoDSClassInfo.GetVariable(outputNode);

                                string varname = string.Concat(outputNodeVarInfo.Name + "." + outputPortVarInfo.Name);
                                if (outputPortVarInfo.DataHolder.IsFunctions) varname += "()";

                                dsGrathViewClass.ClassDrawer.AddInitializeObject
                                    (
                                        name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                        initObjects: varname
                                    );
                            }
                            else
                            {
                                object defaultValue = innerDsClass.ClassDrawer.GetDefaultValue(Type.GetType(innerDsClass.VariableInfo[i].Type));
                                dsGrathViewClass.ClassDrawer.AddInitializeObject
                                    (
                                        mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                        returnRefType: null,
                                        initObjects: defaultValue
                                    );
                            }
                        }
                        if (port.PortSide == PortSide.Output)
                        {
                            List<MethodParamsInfo> inputMethodInfo = new();
                            List<MethodParamsInfo> outputMethodInfo = new();

                            foreach(var variable in innerDsClass.VariableInfo)
                            {
                                var port_ = variable.DataHolder as BasePort;
                                if (port_.PortSide == PortSide.Input)
                                {
                                    MethodParamsInfo methodInfo = new();
                                    methodInfo.ParamName = mainVarInfo.Name + "." + variable.Name;
                                    if (variable.DataHolder.IsFunctions) methodInfo.ParamName = string.Concat(methodInfo.ParamName, "()");
                                    methodInfo.ParamType = Type.GetType(variable.Type);
                                    inputMethodInfo.Add(methodInfo);
                                }
                            }
                            var portNodeScr = node.LambdaGenerationContext(inputMethodInfo.ToArray(), outputMethodInfo.ToArray());

                            dsGrathViewClass.ClassDrawer.AddInitializeLambda
                                (
                                    name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                    context: portNodeScr
                                );
                        }
                    }
                    break;

                case BaseConvertNode convert:
                    innerDsClass = CreateInnerClass(dataHolder);
                    mainVarInfo = AddDSClassToMain(innerDsClass, intoDSClassInfo);

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                    for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                    {
                        string type = innerDsClass.VariableInfo[i].Type;
                        BasePort port = innerDsClass.VariableInfo[i].DataHolder as BasePort;
                        BaseNode node = port.node as BaseNode;

                        if (port.PortSide == PortSide.Input)
                        {
                            if (port.connected)
                            {
                                BasePort outputPort = port.connections.ToList()[0].output as BasePort;
                                BaseNode outputNode = outputPort.node as BaseNode;
                                DSClassInfo outputDsClass = Initialize(outputNode, intoDSClassInfo);
                                VariableInfo outputPortVarInfo = outputDsClass.GetVariable(outputPort);
                                VariableInfo outputNodeVarInfo = intoDSClassInfo.GetVariable(outputNode);

                                string varname = string.Concat(outputNodeVarInfo.Name + "." + outputPortVarInfo.Name);
                                if (outputPortVarInfo.DataHolder.IsFunctions) varname += "()";

                                dsGrathViewClass.ClassDrawer.AddInitializeObject
                                    (
                                        name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                        initObjects: varname
                                    );
                            }
                            else
                            {
                                object defaultValue = innerDsClass.ClassDrawer.GetDefaultValue(Type.GetType(innerDsClass.VariableInfo[i].Type));
                                dsGrathViewClass.ClassDrawer.AddInitializeObject
                                    (
                                        mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                        returnRefType: null,
                                        initObjects: defaultValue
                                    );
                            }
                        }
                        if (port.PortSide == PortSide.Output)
                        {
                            List<MethodParamsInfo> inputMethodInfo = new();
                            List<MethodParamsInfo> outputMethodInfo = new();

                            foreach (var variable in innerDsClass.VariableInfo)
                            {
                                var port_ = variable.DataHolder as BasePort;
                                if (port_.PortSide == PortSide.Input)
                                {
                                    MethodParamsInfo methodInfo = new();
                                    methodInfo.ParamName = mainVarInfo.Name + "." + variable.Name;
                                    if (variable.DataHolder.IsFunctions) methodInfo.ParamName = string.Concat(methodInfo.ParamName, "()");
                                    methodInfo.ParamType = Type.GetType(variable.Type);
                                    inputMethodInfo.Add(methodInfo);
                                }
                            }
                            var portNodeScr = node.LambdaGenerationContext(inputMethodInfo.ToArray(), outputMethodInfo.ToArray());

                            dsGrathViewClass.ClassDrawer.AddInitializeLambda
                                (
                                    name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                    context: portNodeScr
                                );
                        }
                    }
                    break;

                case BaseLogicNode logic:
                    innerDsClass = CreateInnerClass(dataHolder);
                    mainVarInfo = AddDSClassToMain(innerDsClass, intoDSClassInfo);

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                    for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                    {
                        string type = innerDsClass.VariableInfo[i].Type;
                        BasePort port = innerDsClass.VariableInfo[i].DataHolder as BasePort;
                        BaseNode node = port.node as BaseNode;

                        if (port.PortSide == PortSide.Input)
                        {
                            if (port.connected)
                            {
                                BasePort outputPort = port.connections.ToList()[0].output as BasePort;
                                BaseNode outputNode = outputPort.node as BaseNode;
                                DSClassInfo outputDsClass = Initialize(outputNode, intoDSClassInfo);
                                VariableInfo outputPortVarInfo = outputDsClass.GetVariable(outputPort);
                                VariableInfo outputNodeVarInfo = intoDSClassInfo.GetVariable(outputNode);

                                string varname = string.Concat(outputNodeVarInfo.Name + "." + outputPortVarInfo.Name);
                                if (outputPortVarInfo.DataHolder.IsFunctions) varname += "()";

                                dsGrathViewClass.ClassDrawer.AddInitializeObject
                                    (
                                        name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                        initObjects: varname
                                    );
                            }
                            else
                            {
                                object defaultValue = innerDsClass.ClassDrawer.GetDefaultValue(Type.GetType(innerDsClass.VariableInfo[i].Type));
                                dsGrathViewClass.ClassDrawer.AddInitializeObject
                                    (
                                        mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                        returnRefType: null,
                                        initObjects: defaultValue
                                    );
                            }
                        }
                        if (port.PortSide == PortSide.Output)
                        {
                            List<MethodParamsInfo> inputMethodInfo = new();
                            List<MethodParamsInfo> outputMethodInfo = new();

                            foreach (var variable in innerDsClass.VariableInfo)
                            {
                                var port_ = variable.DataHolder as BasePort;
                                if (port_.PortSide == PortSide.Input)
                                {
                                    MethodParamsInfo methodInfo = new();
                                    methodInfo.ParamName = mainVarInfo.Name + "." + variable.Name;
                                    if (variable.DataHolder.IsFunctions) methodInfo.ParamName = string.Concat(methodInfo.ParamName, "()");
                                    methodInfo.ParamType = Type.GetType(variable.Type);
                                    inputMethodInfo.Add(methodInfo);
                                }
                            }
                            var portNodeScr = node.LambdaGenerationContext(inputMethodInfo.ToArray(), outputMethodInfo.ToArray());

                            dsGrathViewClass.ClassDrawer.AddInitializeLambda
                                (
                                    name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                    context: portNodeScr
                                );
                        }
                    }
                    break;

                case BaseLetterNode letter:
                    innerDsClass = CreateInnerClass(dataHolder);
                    mainVarInfo = AddDSClassToMain(innerDsClass, intoDSClassInfo);

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                    for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                    {
                        string type = innerDsClass.VariableInfo[i].Type;
                        dsGrathViewClass.ClassDrawer.AddInitializeObject
                            (
                                mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                returnRefType: null,
                                //paramType: Type.GetType(innerDsClass.Type),
                                paramType: Type.GetType(type),
                                innerDsClass.VariableInfo[i].Value
                            );
                    }
                    break;

                case ActorNode actor:
                    innerDsClass = CreateInnerActorClass(actor);
                    mainVarInfo = AddDSClassActorToMain(innerDsClass, intoDSClassInfo);

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННОЙ ВНУТРЕННЕГО КЛАСОВ ВНУТРИ МЕЙНА
                    string paramName = "actor";
                    if (intoDSClassInfo.ClassDrawer.GetCountParamrters() > 0)
                        paramName = string.Concat(paramName, "_", intoDSClassInfo.ClassDrawer.GetCountParamrters());

                    MethodParamsInfo initMethodInfo = new() {ParamName = paramName, ParamType = actor.ActorType};
                    intoDSClassInfo.ClassDrawer.AddInitializeParameter(initMethodInfo);
                    dsGrathViewClass.ClassDrawer.AddInitializeObject(mainVarInfo.Name + ".Actor", initObjects: paramName );


                    break;

                case BaseDialogueNode dialogue:
                    innerDsClass = CreateInnerDialogueClass(dialogue);
                    mainVarInfo = AddDSClassDialogueToMain(innerDsClass, intoDSClassInfo);

                    if (dialogue is StartDialogueNode startDialogue)
                    {
                        intoDSClassInfo.ClassDrawer.StartDialogueVarname = mainVarInfo.Name;
                    }

                    //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                    for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                    {
                        if (innerDsClass.VariableInfo[i].Name == typeof(DSDialogueOption).Name && innerDsClass.VariableInfo[i].Type == "List<" + GHelper.GetVarType(typeof(DSDialogueOption)) + ">")
                        {
                            //ЗАПОЛНЯЕМ 
                            List<(string text, string dialogueLink, string predicate)> optionsModel = new(); 

                            for (int j = 0; j < innerDsClass.VariableInfo.Count; j++)
                            { 
                                BasePort port = innerDsClass.VariableInfo[j].DataHolder as BasePort;
                                if (port != null)
                                {
                                    switch (port.PortSide)
                                    {
                                        case PortSide.Output:
                                            if (port.connected)
                                            {
                                                BasePort connectedPort = port.connections.ToList()[0].input as BasePort;
                                                BaseNode connectedNode = connectedPort.node as BaseNode;
                                                DSClassInfo outputDsClass = Initialize(connectedNode, intoDSClassInfo);
                                                VariableInfo outputPortVarInfo = outputDsClass.GetVariable(connectedPort);
                                                VariableInfo outputNodeVarInfo = intoDSClassInfo.GetVariable(connectedNode);

                                                //MODEL
                                                (string text, string dialogueLink, string predicate) model = new();
                                                
                                                //TEXT
                                                model.text = port.Value.ToString();
                                                
                                                //PREDICATE
                                                BasePort ifPort = port.IfPortSource;
                                                if (ifPort != null && ifPort.connected)
                                                {
                                                    BasePort connectedToIfPort = ifPort.connections.ToList()[0].output as BasePort;
                                                    BaseNode connectedToIfNode = connectedToIfPort.node as BaseNode;
                                                    DSClassInfo dsClassConnectedToIfNode = Initialize(connectedToIfNode, intoDSClassInfo);
                                                    VariableInfo outputConnectedToIfPortVarInfo = dsClassConnectedToIfNode.GetVariable(connectedToIfPort);
                                                    VariableInfo outputConnectedToIfPortNodeVarInfo = intoDSClassInfo.GetVariable(connectedToIfNode);

                                                    string varname = string.Concat(outputConnectedToIfPortNodeVarInfo.Name + "." + outputConnectedToIfPortVarInfo.Name);
                                                    if (outputConnectedToIfPortVarInfo.DataHolder.IsFunctions) varname += "()";

                                                    model.predicate = string.Concat(" () => ", varname);
                                                }
                                                else model.predicate = string.Empty;

                                                model.dialogueLink = string.Concat(outputNodeVarInfo.Name);
                                                optionsModel.Add(model);
                                            }
                                            break;
                                    }
                                }
                            }

                            List<string> initParams = new();
                            foreach ((string text, string dialogueLink, string predicate) item in optionsModel)
                            {
                                StringBuilder sb = new();

                                sb
                                    .Append(GHelper.BR_OP)
                                    .Append("text: ")
                                    .Append(GHelper.QM)
                                    .Append(item.text)
                                    .Append(GHelper.QM)
                                    .Append(GHelper.COMMA)
                                    .Append(GHelper.SPACE)
                                    .Append("nextDialogues: ")
                                    .Append(item.dialogueLink);

                                if (item.predicate != string.Empty)
                                    sb.Append(GHelper.COMMA)
                                        .Append(GHelper.SPACE)
                                        .Append("func: ")
                                        .Append(item.predicate);
                                
                                sb.Append(GHelper.BR_CL);

                                initParams.Add(sb.ToString());
                            }

                            dsGrathViewClass.ClassDrawer.AddInitializeObject(
                                name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                returnRefType: typeof(List<DSDialogueOption>).FullName,
                                paramType: typeof(DSDialogueOption),
                                //test);
                                initParams.ToArray());
                        }
                        if (innerDsClass.VariableInfo[i].Name == "Text" && innerDsClass.VariableInfo[i].Type == GHelper.GetVarType(typeof(string)))
                        {
                            string t = dialogue.Model.Text;

                            foreach (KeyValuePair<BasePort, string> item in graphView.Anchors)
                            {
                                if (t.Contains(item.Value))
                                {
                                    BaseNode node = item.Key.node as BaseNode;
                                    DSClassInfo init = Initialize(node, intoDSClassInfo);
                                    VariableInfo linkPortVarInfo = init.GetVariable(item.Key);
                                    VariableInfo linkNodeVarInfo = intoDSClassInfo.GetVariable(node);

                                    string varname = string.Concat(linkNodeVarInfo.Name, ".", linkPortVarInfo.Name);
                                    if (linkPortVarInfo.DataHolder.IsFunctions) varname += "()";
                                    t = t.Replace(item.Value, varname);
                                }
                            }

                            dsGrathViewClass.ClassDrawer.AddInitializeObject(
                                name: mainVarInfo.Name + "." + innerDsClass.VariableInfo[i].Name,
                                initObjects: $"@$\"{t}\"");
                        }
                    }
                    break;
            }

            return innerDsClass;
        }
        private DSClassInfo CreateInnerClass(IDataHolder dataHolder)
        {
            //СОЗДАНИЕ DSCLASS ПОД ОТДЕЛЬННУЮ НОДУ
            Type genericType = typeof(DSClassInfo<>).MakeGenericType(dataHolder.GetType());
            ConstructorInfo constructor = genericType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(VisualElement) }, null);
            object instance = constructor.Invoke(new object[] { dataHolder as VisualElement });
            DSClassInfo innerDsClass = (DSClassInfo)instance;
            innerDsClass.IDataHolder = dataHolder;

            //СОЗДАНИЕ ВНУТРЕННИХ ПЕРЕМЕННЫХ ДЛЯ ЭКЗЕМПЛЯРА ВНУТРЕННОГО КЛАССА
            foreach (var item in innerDsClass.DataHolders)
            {
                string fieldname = item.Name;
                var info = new VariableInfo()
                {
                    ClassInfo = innerDsClass,
                    DataHolder = item,
                    Value = item.Value,
                    Name = fieldname + $"_{innerDsClass.VariableInfo.Count}",
                    Type = item.Type.FullName,
                    Visibility = Visibility.@public,
                };
                innerDsClass.VariableInfo.Add(info);
            }

            //ЗАДАЕМ ЗНАЧЕНИЕ ПОЛЯМ ДЛЯ ВНУТРЕННИХ КЛАССОВ
            for (int i = 0; i < innerDsClass.VariableInfo.Count; i++)
                innerDsClass.VariableInfo[i].Value = innerDsClass.DataHolders[i].Value;

            return innerDsClass;
        }
        private DSClassInfo CreateInnerActorClass(ActorNode actorNode)
        {
            //СОЗДАНИЕ DSCLASS ПОД ОТДЕЛЬННУЮ НОДУ
            Type genericType = typeof(DSClassInfo<>).MakeGenericType(actorNode.GetType());
            ConstructorInfo constructor = genericType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(VisualElement) }, null);
            object instance = constructor.Invoke(new object[] { actorNode as VisualElement });
            DSClassInfo innerDsClass = (DSClassInfo)instance;
            innerDsClass.IDataHolder = actorNode;

            //ЗАДАЕМ АДЫКВАТНЫЙ ТИП ДЛЯ КЛАССА-ОБЕРТКИ АКТОРА
            innerDsClass.SetType(actorNode.GetType().Name);
            innerDsClass.AddTypePefix("_" + actorNode.ActorType.Name);

            foreach (var item in innerDsClass.DataHolders)
            {
                string fieldname = item.Name;
                var info = new VariableInfo()
                {
                    ClassInfo = innerDsClass,
                    DataHolder = item,
                    Value = item.Value,
                    Name = "Actor." + GHelper.RemoveSpacesAndContentBetweenParentheses(fieldname),
                    Type = item.Type.FullName,
                    Visibility = Visibility.@public,
                };
                innerDsClass.VariableInfo.Add(info);
            }

            return innerDsClass;
        }
        private DSClassInfo CreateInnerDialogueClass(BaseDialogueNode dialogueNode)
        {
            //СОЗДАНИЕ DSCLASS ПОД ОТДЕЛЬННУЮ НОДУ
            Type genericType = typeof(DSClassInfo<>).MakeGenericType(dialogueNode.GetType());
            ConstructorInfo constructor = genericType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(VisualElement) }, null);
            object instance = constructor.Invoke(new object[] { dialogueNode as VisualElement });
            DSClassInfo innerDsClass = (DSClassInfo)instance;
            innerDsClass.IDataHolder = dialogueNode;

            //ЗАДАЕМ АДЫКВАТНЫЙ ТИП ДЛЯ КЛАССА-ОБЕРТКИ АКТОРА
            innerDsClass.SetType(typeof(DSDialogue).Name);

            //СОЗДАНИЕ ВНУТРЕННИХ ПЕРЕМЕННЫХ ДЛЯ ЭКЗЕМПЛЯРА ВНУТРЕННОГО КЛАССА
            foreach (var item in innerDsClass.DataHolders)
            {
                string fieldname = item.Name;
                var info = new VariableInfo()
                {
                    ClassInfo = innerDsClass,
                    DataHolder = item,
                    Value = item.Value,
                    Name = fieldname + $"_{innerDsClass.VariableInfo.Count}",
                    Type = GHelper.GetVarType(item.Type),
                    Visibility = Visibility.@public,
                };
                innerDsClass.VariableInfo.Add(info);
            }
            VariableInfo text = new VariableInfo()
            {
                Type = GHelper.GetVarType(typeof(string)),
                Name = "Text",
                Visibility = Visibility.@public,
            };
            VariableInfo options = new VariableInfo()
            {
                Type = "List<" + GHelper.GetVarType(typeof(DSDialogueOption)) + ">",
                Name = typeof(DSDialogueOption).Name,
                Visibility = Visibility.@public,
            };
            innerDsClass.VariableInfo.Add(text);
            innerDsClass.VariableInfo.Add(options);

            return innerDsClass;
        }
        

        private VariableInfo AddDSClassToMain(DSClassInfo innerDsClass, DSClassInfo<DSGraphView> intoDSClassInfo)
        {
            //ИНИЦИАЛИЗАЦИЯ СИГНАТУРЫ ВНУТРЕННЕГО КЛАССА
            if (intoDSClassInfo.RegisterInnerClassDeclaration(innerDsClass))
            {
                var classDrawer = innerDsClass.ClassDrawer;
                classDrawer.ClassDeclaration(innerDsClass.Type, Attribute.SystemSerializable, Visibility.@private);
                foreach (var info in innerDsClass.VariableInfo)
                    classDrawer.AddField(info, attribute: Attribute.SerializeField, false);
            }

            //СОЗДАНИЕ ПЕРЕМЕННОЙ ДЛЯ ГЛАВНОГО КЛАССА
            VariableInfo mainVarInfo = new VariableInfo()
            {
                ClassInfo = innerDsClass,
                DataHolder = innerDsClass.IDataHolder,
                Visibility = Visibility.@private,
                Type = innerDsClass.Type,
                Name = innerDsClass.Type + "_" + intoDSClassInfo.VariableInfo.Where(e => e.Type == innerDsClass.Type).Count(),
                Value = null,
            };
            dsGrathViewClass.VariableInfo.Add(mainVarInfo);

            //ДОБАВЛЕНИЕ ПОЛЯ ВНУТРЕННИХ КЛАССОВ В МЕЙН
            dsGrathViewClass.ClassDrawer.AddField(mainVarInfo, attribute: Attribute.SerializeField, true);

            return mainVarInfo;
        }
        private VariableInfo AddDSClassActorToMain(DSClassInfo innerDsClass, DSClassInfo<DSGraphView> intoDSClassInfo)
        {
            //ИНИЦИАЛИЗАЦИЯ СИГНАТУРЫ ВНУТРЕННЕГО КЛАССА
            ActorNode actorNode = innerDsClass.IDataHolder as ActorNode;
            if (intoDSClassInfo.RegisterInnerClassDeclaration(innerDsClass))
            {
                var classDrawer = innerDsClass.ClassDrawer;
                classDrawer.ClassDeclaration(innerDsClass.Type, Attribute.SystemSerializable, Visibility.@private);
                classDrawer.AddField("Actor", actorNode.ActorType.FullName, Attribute.SerializeField, visibility: Visibility.@public, isNew: false, null);
            }

            //СОЗДАНИЕ ПЕРЕМЕННОЙ ДЛЯ ГЛАВНОГО КЛАССА
            VariableInfo mainVarInfo = new VariableInfo()
            {
                ClassInfo = innerDsClass,
                DataHolder = innerDsClass.IDataHolder,
                Visibility = Visibility.@private,
                Type = innerDsClass.Type,
                Name = innerDsClass.Type + "_" + intoDSClassInfo.VariableInfo.Where(e => e.Type == innerDsClass.Type).Count(),
                Value = null,
            };
            dsGrathViewClass.VariableInfo.Add(mainVarInfo);

            //ДОБАВЛЕНИЕ ПОЛЯ ВНУТРЕННИХ КЛАССОВ В МЕЙН
            dsGrathViewClass.ClassDrawer.AddField(mainVarInfo, attribute: Attribute.SerializeField, true);

            return mainVarInfo;
        }
        private VariableInfo AddDSClassDialogueToMain(DSClassInfo innerDsClass, DSClassInfo<DSGraphView> intoDSClassInfo)
        {
            BaseDialogueNode baseDialogueNode = innerDsClass.IDataHolder as BaseDialogueNode;

            //СОЗДАНИЕ ПЕРЕМЕННОЙ ДЛЯ ГЛАВНОГО КЛАССА
            VariableInfo mainVarInfo = new VariableInfo()
            {
                ClassInfo = innerDsClass,
                DataHolder = innerDsClass.IDataHolder,
                Visibility = Visibility.@private,
                Type = innerDsClass.Type,
                Name = innerDsClass.Type + "_" + intoDSClassInfo.VariableInfo.Where(e => e.Type == innerDsClass.Type).Count(),
                Value = null,
            };
            dsGrathViewClass.VariableInfo.Add(mainVarInfo);

            //ДОБАВЛЕНИЕ ПОЛЯ ВНУТРЕННИХ КЛАССОВ В МЕЙН
            dsGrathViewClass.ClassDrawer.AddField(mainVarInfo, attribute: Attribute.SerializeField, true);

            return mainVarInfo;
        }

        internal ClassDrawer GetDrawer()
        {
            dsGrathViewClass.ClassDrawer.ClassDeclaration(dsGrathViewClass.ClassName, Attribute.None, Visibility.@public, new System.Type[] { typeof(DialogueDisposer) });
            foreach (var item in dsGrathViewClass.InnerClassInfo)
                dsGrathViewClass.ClassDrawer.AddInnerClass(item.ClassDrawer);

            return dsGrathViewClass.ClassDrawer;
        }
    }
}
