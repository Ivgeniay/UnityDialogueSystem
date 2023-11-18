using System;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine.UIElements;

namespace DialogueSystem.Generators
{
    internal static class Ext
    {
        internal static ICLassDrawer AddProperty(this ICLassDrawer cd, string propertyName, Type returnType, Attribute attribute, Visibility visibility, object value = null) =>
            cd.AddProperty(propertyName, returnType.FullName, attribute, visibility, value);
        internal static ICLassDrawer AddField(this ICLassDrawer cd, string propertyName, Type returnType, Attribute attribute, Visibility visibility, object value = null) =>
            cd.AddField(propertyName, returnType.FullName, attribute, visibility);


        public static List<T> GetElementsByType<T>(this VisualElement root, Func<object, bool> predicate = null)
        {
            List<T> elementsOfType = new List<T>();
            CollectElementsByType(root, elementsOfType, predicate);

            return elementsOfType;
        }

        private static void CollectElementsByType<T>(VisualElement element, List<T> elementsOfType, Func<object, bool> predicate = null)
        {
            if (element is T typedElement && predicate(typedElement))
            {
                elementsOfType.Add(typedElement);
            }

            foreach (var child in element.Children())
            {
                CollectElementsByType(child, elementsOfType, predicate);
            }
        }
    }
}
