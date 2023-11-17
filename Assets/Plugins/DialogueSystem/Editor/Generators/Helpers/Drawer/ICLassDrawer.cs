using System;

namespace DialogueSystem.Generators
{
    internal interface ICLassDrawer
    {
        public ICLassDrawer ClassDeclaration(string className, Attribute attribute, Visibility visibility, Type[] inherets = null);
        public ICLassDrawer AddProperty(string propertyName, string returnType, Attribute attribute, Visibility visibility, object value = null);
        public ICLassDrawer AddField(string fieldName, string returnType, Attribute attribute, Visibility visibility, object value = null);
        public ICLassDrawer AddInnerClass(ClassDrawer classDrawer);
        public ICLassDrawer AddInitializeObject(string name, string returnType, params object[] initObjects);
        public ICLassDrawer ClassCtor();
        public ICLassDrawer AddMethod(string methodName, Attribute attribute, Visibility visibility, string returnType = null, MethodParamsInfo[] parameters = null, string value = null);
        public string Draw();
    }
}
