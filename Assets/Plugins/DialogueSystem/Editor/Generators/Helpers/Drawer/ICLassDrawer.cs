using System;

namespace DialogueSystem.Generators
{
    internal interface ICLassDrawer
    {
        public ICLassDrawer ClassDeclaration(string className, Attribute attribute, Visibility visibility, Type[] inherets = null);
        public ICLassDrawer AddProperty(string propertyName, string returnType, Attribute attribute, Visibility visibility, object value = null);
        public ICLassDrawer AddField(string fieldName, string returnType, Attribute attribute, Visibility visibility, bool isNew = true, object value = null);
        public ICLassDrawer AddInnerClass(ClassDrawer classDrawer);
        public ICLassDrawer AddInitializeLambda(string name, string context);
        public ICLassDrawer AddInitializeObject(string name, params object[] initObjects);
        public ICLassDrawer AddInitializeObject(string name, string returnType = null, Type paramType = null, params object[] initObjects);
        public ICLassDrawer ClassCtor();
        public ICLassDrawer AddMethod(string methodName, Attribute attribute, Visibility visibility, string returnType = null, MethodParamsInfo[] parameters = null, string value = null);
        public string Draw();
    }
}
