using System;

namespace DialogueSystem.Generators
{
    internal static class Ext
    {
        internal static ICLassDrawer AddProperty(this ICLassDrawer cd, string propertyName, Type returnType, Attribute attribute, Visibility visibility, object value = null) =>
            cd.AddProperty(propertyName, returnType.FullName, attribute, visibility, value);
        internal static ICLassDrawer AddField(this ICLassDrawer cd, string propertyName, Type returnType, Attribute attribute, Visibility visibility, object value = null) =>
            cd.AddField(propertyName, returnType.FullName, attribute, visibility);
    }
}
