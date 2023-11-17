using Codice.CM.Common.Zlib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace DialogueSystem.Generators
{
    internal class ClassDrawer : ICLassDrawer
    {
        private string ClassName = "";
        private Visibility ClassVisibility = Visibility.None;
        public StringBuilder DeclarationClass { get; private set; } 
        public StringBuilder CtorClass { get; private set; } 
        public List<StringBuilder> Propertyes { get; private set; } = new();
        public List<StringBuilder> Fields { get; private set; } = new();
        public List<StringBuilder> InnerClasses { get; private set; } = new();
        public List<StringBuilder> Methods { get; private set; } = new();
        public List<StringBuilder> InitializeObjects { get; private set; } = new();

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
        public ICLassDrawer AddField(string fieldName, string returnType, Attribute attribute, Visibility visibility, object value = null)
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
        public ICLassDrawer AddInitializeObject(string name, string returnType, params object[] initObjects)
        {
            StringBuilder initObj = new();

            initObj
                .Append(name)
                .Append(GHelper.SPACE)
                .Append(GHelper.EQLS)
                .Append(GHelper.SPACE)
                .Append(GHelper.NEW)
                .Append(GHelper.SPACE)
                .Append(GHelper.GetVarType(Type.GetType(returnType)))
                .Append(GHelper.BR_F_OP);

            for (int i = 0; i < initObjects.Length; i++)
            {
                var type = initObjects[i].GetType();
                if (type.IsValueType) initObj.Append(GHelper.GetValueWithPrefix(type, initObjects[i]));
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

            initObj
                .Append(GHelper.BR_F_CL)
                .Append(GHelper.QUOTES);

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
            StringBuilder method = new();
            method
                .Append(GHelper.GetVisibility(ClassVisibility))
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

        public string Draw()
        {
            StringBuilder classBuilder = new();

            classBuilder
                .Append(DeclarationClass)
                .Append(GHelper.BR_F_OP);

            if (Propertyes != null && Propertyes.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(Propertyes)).Append(GHelper.TR);
            foreach (var item in Propertyes) classBuilder.Append(item);
            if (Propertyes != null && Propertyes.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);
            
            if (Fields != null && Fields.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(Fields)).Append(GHelper.TR);
            foreach (var item in Fields) classBuilder.Append(item);
            if (Fields != null && Fields.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);

            if (InitializeObjects != null && InitializeObjects.Count > 0)
            {
                StringBuilder valueToInitMethod = new();
                foreach (var item in InitializeObjects)
                    valueToInitMethod.AppendLine(item.ToString());

                AddMethod("Initialize", Attribute.None, Visibility.@private, null, null, valueToInitMethod.ToString());
            }

            if (Methods != null && Methods.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(Methods)).Append(GHelper.TR);
            foreach (var item in Methods) classBuilder.Append(item);
            if (Methods != null && Methods.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);

            if (InnerClasses != null && InnerClasses.Count > 0) classBuilder.Append(GHelper.REG).Append(GHelper.SPACE).Append(nameof(InnerClasses)).Append(GHelper.TR);
            foreach (var item in InnerClasses) classBuilder.Append(item).Append(GHelper.TR);
            if (InnerClasses != null && InnerClasses.Count > 0) classBuilder.Append(GHelper.ENDREG).Append(GHelper.TR);

            classBuilder.Append(GHelper.BR_F_CL);

            return classBuilder.ToString();
        }
    }
}
