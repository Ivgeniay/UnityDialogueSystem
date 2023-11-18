using DialogueSystem.Abstract;
using DialogueSystem.Nodes;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace DialogueSystem.Generators
{
    internal class ClassGenerator : GHelper
    {
        DSClassInfo<DSGraphView> dsGrathViewClass = null;
        ClassDrawer mainClassDrawer;

        internal ClassGenerator(DSGraphView grathView, string className)
        {
            dsGrathViewClass = new(grathView);
            dsGrathViewClass.ClassName = className;
            mainClassDrawer = new();
        }

        internal void Initialize()
        {
            dsGrathViewClass.Initialize();

            foreach (IDataHolder dataHolder in dsGrathViewClass.DataHolders)
            {
                Type genericType = typeof(DSClassInfo<>).MakeGenericType(dataHolder.GetType());
                ConstructorInfo constructor = genericType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(VisualElement) }, null);
                object instance = constructor.Invoke(new object[] { dataHolder as VisualElement });
                DSClassInfo dsClass = (DSClassInfo)instance;

                dsClass.Initialize();
                foreach (var item in dsClass.DataHolders)
                {
                    string fieldname = item.Name;
                    var info = new VariableInfo()
                    {
                        Name = fieldname + $"_{dsClass.VariableInfo.Count}",
                        Type = item.Type.FullName,
                        Visibility = Visibility.@public,
                    };
                    dsClass.VariableInfo.Add(info);
                }

                if (dsGrathViewClass.RegisterInnerClass(dsClass))
                {
                    //СОЗДАНИЕ ИНИЦИАЛИЗАЦИИ ВНУТРЕННЕГО КЛАССА
                    var classDrawer = dsClass.ClassDrawer;
                    classDrawer.ClassDeclaration(dsClass.Type, Attribute.SystemSerializable, Visibility.@private);
                    foreach (var info in dsClass.VariableInfo)
                        classDrawer.AddField(info.Name, info.Type, Attribute.SerializeField, info.Visibility);
                }

                switch(dataHolder)
                {
                    case BasePrimitiveNode primitive:
                        //ЗАДАЕМ ЗНАЧЕНИЕ ПОЛЯМ ДЛЯ ВНУТРЕННИХ КЛАССОВ
                        for (int i = 0; i < dsClass.VariableInfo.Count; i++)
                            dsClass.VariableInfo[i].Value = dsClass.DataHolders[i].Value;

                        //СОЗДАНИЕ ПЕРЕМЕННОЙ ДЛЯ ГЛАВНОГО КЛАССА
                        var mainVarInfo = new VariableInfo()
                        {
                            ClassInfo = dsClass,
                            Visibility = Visibility.@public,
                            Type = dsClass.Type,
                            Name = dsClass.Type + "_" + dsGrathViewClass.VariableInfo.Where(e => e.Type == dsClass.Type).Count(),
                        };
                        dsGrathViewClass.VariableInfo.Add(mainVarInfo);

                        //ДОБАВЛЕНИЕ ПОЛЯ ВНУТРЕННИХ КЛАССОВ В МЕЙН
                        mainClassDrawer.AddField(mainVarInfo, attribute: Attribute.SerializeField);

                        //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                        for (int i = 0; i < dsClass.VariableInfo.Count; i++)
                        {
                            var type = dsClass.VariableInfo[i].Type;
                            mainClassDrawer.AddInitializeObject(
                                mainVarInfo.Name + "." + dsClass.VariableInfo[i].Name,
                                null,
                                paramType: Type.GetType(type),
                                dsClass.VariableInfo[i].Value);
                        }
                        break;

                    case BaseOperationNode operation:
                        break;

                    case BaseConvertNode convert:
                        break;

                    case BaseLogicNode logic:
                        break;

                    case BaseLetterNode letter:
                        //ЗАДАЕМ ЗНАЧЕНИЕ ПОЛЯМ ДЛЯ ВНУТРЕННИХ КЛАССОВ
                        for (int i = 0; i < dsClass.VariableInfo.Count; i++)
                            dsClass.VariableInfo[i].Value = dsClass.DataHolders[i].Value;

                        //СОЗДАНИЕ ПЕРЕМЕННОЙ ДЛЯ ГЛАВНОГО КЛАССА
                        mainVarInfo = new VariableInfo()
                        {
                            ClassInfo = dsClass,
                            Visibility = Visibility.@public,
                            Type = dsClass.Type,
                            Name = dsClass.Type + "_" + dsGrathViewClass.VariableInfo.Where(e => e.Type == dsClass.Type).Count(),
                        };
                        dsGrathViewClass.VariableInfo.Add(mainVarInfo);

                        //ДОБАВЛЕНИЕ ПОЛЯ ВНУТРЕННИХ КЛАССОВ В МЕЙН
                        mainClassDrawer.AddField(mainVarInfo, attribute: Attribute.SerializeField);

                        //ИНИЦИАЛИЗАЦИЯ ПЕРЕМЕННЫХ ВНУТРЕННИХ КЛАСОВ ВНУТРИ МЕЙНА
                        for (int i = 0; i < dsClass.VariableInfo.Count; i++)
                        {
                            mainClassDrawer.AddInitializeObject(
                                mainVarInfo.Name + "." + dsClass.VariableInfo[i].Name,
                                null,
                                paramType: Type.GetType(dsClass.Type),
                                dsClass.VariableInfo[i].Value);
                        }
                        break;

                    case BaseDialogueNode dialogue:
                        break;

                }
            }
        }

        private string GetInnerMainClassVariable(DSClassInfo innerClassInfo)
        {
            return dsGrathViewClass.VariableInfo.First(e => e.Type == innerClassInfo.Type).Name;
        }

        internal ClassDrawer GetDrawer()
        {
            mainClassDrawer.ClassDeclaration(dsGrathViewClass.ClassName, Attribute.None, Visibility.@public, new System.Type[] { typeof(DialogueDisposer) });


                //.AddProperty("Prop", typeof(string).FullName, Attribute.SerializeField, Visibility.@public, "dfsa")
                //.AddField("Field1", typeof(string).FullName, Attribute.SerializeField, Visibility.@public, "kek")
                //.AddInitializeObject("lib", typeof(List<int>).FullName, 55.45d, 32d, 12d, 233d)
                //.AddMethod("Method1", Attribute.SystemSerializable, Visibility.@public, null, new MethodParamsInfo[]
                //{
                //    new MethodParamsInfo()
                //    {
                //        ParamName = "kek",
                //        ParamType = typeof(string),
                //    },
                //    new MethodParamsInfo()
                //    {
                //        ParamName = "age",
                //        ParamType = typeof(double),
                //    }
                //}
                //, "XYU");

            foreach (var item in dsGrathViewClass.InnerClassInfo)
            {
                mainClassDrawer.AddInnerClass(item.ClassDrawer);
            }

            return mainClassDrawer;
        }
    }
}
