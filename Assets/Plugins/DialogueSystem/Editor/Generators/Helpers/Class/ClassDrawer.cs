using Codice.CM.Common.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UIElements;
using static DialogueSystem.DialogueDisposer.DSDialogueOption;

namespace DialogueSystem.Generators
{
    internal class ClassDrawer : ICLassDrawer
    {
        private string ClassName = "";
        private Visibility ClassVisibility = Visibility.None;
        internal StringBuilder DeclarationClass { get; private set; }
        internal StringBuilder CtorClass { get; private set; } 
        internal List<StringBuilder> Propertyes { get; private set; } = new();
        internal List<StringBuilder> Fields { get; private set; } = new();
        internal List<StringBuilder> InnerClasses { get; private set; } = new();
        internal List<StringBuilder> Methods { get; private set; } = new();
        internal List<StringBuilder> InitializeObjects { get; private set; } = new();
        internal string StartDialogueVarname = string.Empty;

        private List<MethodParamsInfo> InitializeParameters = null;
        private List<MethodInfo> InitializeMethods = null;


        public ICLassDrawer ClassDeclaration(string className, Attribute attribute, Visibility visibility, Type[] inherets = null)
        {
            this.ClassName = className;
            this.ClassVisibility = visibility;
            DeclarationClass = new();

            if (attribute != Attribute.None)
            {
                DeclarationClass
                    .Append(GHelper.GetAttribute(attribute))
                    .Append(GHelper.SPACE);
            }

            DeclarationClass
                .Append(GHelper.GetVisibility(visibility))
                .Append(GHelper.SPACE)
                .Append("class")
                .Append(GHelper.SPACE)
                .Append(className);

            if (inherets != null)
            {
                DeclarationClass
                .Append(GHelper.SPACE)
                .Append(GHelper.COLON)
                .Append(GHelper.SPACE);
                for (int i = 0; i < inherets.Length; i++)
                {
                    DeclarationClass.Append(inherets[i].FullName);
                    if (i < inherets.Length - 1) DeclarationClass.Append(GHelper.COMMA).Append(GHelper.SPACE);
                }
            }

            return this;
        }
        public ICLassDrawer AddProperty(string propertyName, string returnType, Attribute attribute, Visibility visibility, object value = null)
        {
            StringBuilder property = new();
            Type type = Type.GetType(returnType);

            property
                .Append(GHelper.GetAttribute(attribute))
                .Append(GHelper.SPACE)
                .Append(GHelper.GetVisibility(visibility))
                .Append(GHelper.SPACE)
                .Append(returnType)
                .Append(GHelper.SPACE)
                .Append(propertyName)
                .Append(GHelper.SPACE)
                .Append(GHelper.APROP);

            if (value != null)
            {
                property
                    .Append(GHelper.SPACE)
                    .Append(GHelper.EQLS)
                    .Append(GHelper.SPACE)
                    .Append(GHelper.GetValueWithPrefix(type, value));
                
                property.Append(GHelper.QUOTES);
            }

            property.Append(GHelper.TR);

            Propertyes.Add(property);
            return this;
        }
        public ICLassDrawer AddField(VariableInfo variable, Attribute attribute, bool isNew = true, object value = null)
        {
            string type = variable.Type;
            if (variable.DataHolder != null && variable.DataHolder.IsFunctions)
            {
                StringBuilder funcBuilder = GetFunc(variable.Type);
                type = funcBuilder.ToString();
            }
            return AddField(variable.Name, type, attribute, variable.Visibility, isNew, value);
        }
        public ICLassDrawer AddField(string fieldName, string returnType, Attribute attribute, Visibility visibility, bool isNew = true, object value = null)
        {
            StringBuilder field = new();
            Type type = Type.GetType(returnType);

            field
                .Append(GHelper.GetAttribute(attribute))
                .Append(GHelper.SPACE)
                .Append(GHelper.GetVisibility(visibility))
                .Append(GHelper.SPACE)
                .Append(returnType)
                .Append(GHelper.SPACE)
                .Append(fieldName);
            
            if (isNew)
            {
                field
                    .Append(GHelper.SPACE)
                    .Append(GHelper.EQLS)
                    .Append(GHelper.SPACE)
                    .Append(GHelper.NEW)
                    .Append(GHelper.BR_OP)
                    .Append(GHelper.BR_CL);
            }

            if (value != null)
            {
                field
                    .Append(GHelper.SPACE)
                    .Append(GHelper.EQLS)
                    .Append(GHelper.SPACE)
                    .Append(GHelper.GetValueWithPrefix(type, value));
            }
            field.Append(GHelper.QUOTES).Append(GHelper.TR); ;

            Fields.Add(field);
            return this;
        }
        public ICLassDrawer AddInnerClass(ClassDrawer classDrawer)
        {
            InnerClasses.Add(new StringBuilder(classDrawer.Draw()));
            return this;
        }
        public ICLassDrawer AddInitializeLambda(string name, string context)
        {
            StringBuilder initObj = new();

            initObj
                .Append(name)
                .Append(GHelper.SPACE)
                .Append(GHelper.EQLS)
                .Append(GHelper.SPACE)
                .Append(GHelper.BR_OP)
                .Append(GHelper.BR_CL)
                .Append(GHelper.SPACE)
                .Append(GHelper.EQLS)
                .Append(GHelper.R_TRIANGE)
                .Append(GHelper.BR_F_OP)
                .Append(context)
                .Append(GHelper.BR_F_CL)
                .Append(GHelper.QUOTES);

            InitializeObjects.Add(initObj);
            return this;
        }
        public ICLassDrawer AddInitializeObject(string name, params object[] initObjects)
        {
            StringBuilder initObj = new();

            initObj
                .Append(name)
                .Append(GHelper.SPACE)
                .Append(GHelper.EQLS)
                .Append(GHelper.SPACE);

            for (int i = 0; i < initObjects.Length; i++)
            {
                initObj.Append(initObjects[i]);
                if (i < initObjects.Length - 1) initObj.Append(GHelper.COMMA).Append(GHelper.TR);
            }

            initObj.Append(GHelper.QUOTES);

            InitializeObjects.Add(initObj);
            return this;
        }
        public ICLassDrawer AddInitializeObject(string name, string returnRefType = null, Type paramType = null, params object[] initObjects)
        {
            StringBuilder initObj = new();

            initObj
                .Append(name)
                .Append(GHelper.SPACE)
                .Append(GHelper.EQLS)
                .Append(GHelper.SPACE);

            if (returnRefType != null)
            {
                initObj.Append(GHelper.NEW)
                    .Append(GHelper.SPACE)
                    .Append(GHelper.GetVarType(Type.GetType(returnRefType)))
                    .Append(GHelper.BR_F_OP);
            }

            for (int i = 0; i < initObjects.Length; i++)
            {
                Type type = paramType;
                if (type == null) type = initObjects[i].GetType();

                if (type.IsValueType || type == typeof(string))
                {
                    var valueWithPrefix = GHelper.GetValueWithPrefix(type, initObjects[i]);
                    initObj.Append(valueWithPrefix);
                }
                else
                {
                    initObj
                        .Append(GHelper.NEW)
                        .Append(GHelper.SPACE)
                        .Append(GHelper.GetVarType(type))
                        .Append(initObjects[i]);
                }
                if (i < initObjects.Length - 1) initObj.Append(GHelper.COMMA).Append(GHelper.TR);
            }

            if (returnRefType != null)
                initObj.Append(GHelper.BR_F_CL);

            initObj.Append(GHelper.QUOTES);

            InitializeObjects.Add(initObj);
            return this;
        }
        public ICLassDrawer ClassCtor()
        {
            //DeclarationClass
            //    .Append(GHelper.GetVisibility(ClassVisibility))
            //    .Append(GHelper.SPACE)
            //    .Append(ClassName);
            return this;
        }
        public ICLassDrawer AddMethod(string methodName, Attribute attribute, Visibility visibility, string returnType = null, MethodParamsInfo[] parameters = null, string value = null)
        {
            if (InitializeMethods == null) InitializeMethods = new();
            InitializeMethods.Add(new MethodInfo()
            {
                Visibility = visibility,
                Name = methodName,
                Parameters = parameters,
                ReturnParameters = returnType == null ? null : new[] { returnType },
            });

            StringBuilder method = new();
            method
                .Append(GHelper.GetVisibility(visibility))
                .Append(GHelper.SPACE);

            if (returnType == null) method.Append("void");
            else method.Append(returnType);

            method
                .Append(GHelper.SPACE)
                .Append(methodName)
                .Append(GHelper.BR_OP);

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    method.Append(parameters[i].ParamType.FullName).Append(GHelper.SPACE).Append(parameters[i].ParamName);
                    if (i < parameters.Length - 1) method.Append(", ");
                }
            }

            method.Append(GHelper.BR_CL)
                .Append(GHelper.BR_F_OP)
                .Append(value)
                .Append(GHelper.BR_F_CL)
                .Append(GHelper.TR);


            Methods.Add(method);
            return this;
        }
        public string CallMethod(string MethodName, params MethodParamsInfo[] parameters)
        {
            StringBuilder methodCall = new();

            MethodInfo info = InitializeMethods.FirstOrDefault(m => m.Name == MethodName);
            if (info != null)
            {
                methodCall
                    .Append(info.Name)
                    .Append(GHelper.BR_OP);

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        methodCall.Append(parameters[i].ParamName);
                        if (i < parameters.Length - 1) methodCall.Append(", ");
                    }
                }
                methodCall
                    .Append(GHelper.BR_CL)
                    .Append(GHelper.QUOTES);
            }

            return methodCall.ToString();
        }
        public ICLassDrawer AddInitializeParameter(MethodParamsInfo methodParamsInfo)
        {
            if (InitializeParameters == null) InitializeParameters = new();
            if (!InitializeParameters.Contains(methodParamsInfo)) InitializeParameters.Add(methodParamsInfo);
            return this;
        }
        public int GetCountParamrters()
        {
            if (InitializeParameters == null) return 0;
            else return InitializeParameters.Count;
        }

        public StringBuilder GetFunc(params string[] types)
        {
            StringBuilder func = new();
            func.Append("Func<");
            for (int i = 0; i < types.Length; i++)
            {
                func.Append(types[i]);
                if (i < types.Length - 1) func.Append(", ");
            }
            func.Append(">");
            return func;
        }
        public object GetDefaultValue(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type.IsValueType) return Activator.CreateInstance(type);
            else if (type == typeof(string)) return string.Empty;
            else return null;
        }
        public StringBuilder GetDefaultValue(Type type, bool isDeclaration = false)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            StringBuilder builder = new();

            if (type.IsValueType)
            {
                object defaultValue = Activator.CreateInstance(type);
                if (isDeclaration) builder.Append(GHelper.GetValueWithPrefix(type, defaultValue));
            }
            else if (type == typeof(string)) builder.Append("");
            else builder.Append("null");
            return builder;
        }

        public string Draw()
        {
            StringBuilder classBuilder = new();

            classBuilder
                .Append(DeclarationClass)
                .Append(GHelper.BR_F_OP);

            if (Propertyes != null && Propertyes.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(Propertyes)).Append(GHelper.TR);
            foreach (StringBuilder item in Propertyes) classBuilder.Append(item);
            if (Propertyes != null && Propertyes.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);
            
            if (Fields != null && Fields.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(Fields)).Append(GHelper.TR);
            foreach (StringBuilder item in Fields) classBuilder.Append(item);
            if (Fields != null && Fields.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);

            if (InitializeObjects != null && InitializeObjects.Count > 0)
            {
                StringBuilder valueToInitMethod = new(); 
                foreach (var item in InitializeObjects) valueToInitMethod.AppendLine(item.ToString());
                AddMethod(
                    "Initialize", 
                    Attribute.None, 
                    Visibility.@private, 
                    null, 
                    InitializeParameters == null ? null : InitializeParameters.ToArray(), 
                    valueToInitMethod.ToString());

                StringBuilder startDialogueValue = new StringBuilder()
                    .AppendLine(CallMethod("Initialize", InitializeParameters == null ? null : InitializeParameters.ToArray()))
                    .AppendLine($"return {StartDialogueVarname};");

                AddMethod(
                    "StartDialogue",
                    Attribute.None,
                    Visibility.@public,
                    GHelper.GetVarType(typeof(DSDialogue)),
                    InitializeParameters == null ? null : InitializeParameters.ToArray(),
                    startDialogueValue.ToString());
            }

            if (Methods != null && Methods.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(Methods)).Append(GHelper.TR);
            foreach (StringBuilder item in Methods) classBuilder.Append(item);
            if (Methods != null && Methods.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);

            if (InnerClasses != null && InnerClasses.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(InnerClasses)).Append(GHelper.TR);
            foreach (StringBuilder item in InnerClasses) classBuilder.Append(item).Append(GHelper.TR);
            if (InnerClasses != null && InnerClasses.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);

            classBuilder.Append(GHelper.BR_F_CL);

            return classBuilder.ToString();
        }
    }
}
