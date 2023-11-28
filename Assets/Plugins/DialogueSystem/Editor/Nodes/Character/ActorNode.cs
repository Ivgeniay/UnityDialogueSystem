using System.Collections.Generic;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using DialogueSystem.Nodes;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using DialogueSystem.Utilities;

namespace DialogueSystem
{
    public class ActorNode : BaseNode
    {
        public Type ActorType { 
            get
            {
                string tyStr = this.Model.Text;
                Type ty = Type.GetType(tyStr);
                if (ty == null)
                {
                    string assemblyFullName = "Assembly-CSharp";
                    Assembly assembly = Assembly.Load(assemblyFullName);
                    ty = assembly.GetType(tyStr);
                }
                return ty;
            }
        }

        private Dictionary<string, Type> publicFields = new(); 
        private Dictionary<string, Type> publicProperties = new();

        Dictionary<string, TypeInfo> fieldsInfo = new();
        Dictionary<string, TypeInfo> propertyInfo = new();

        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);
        }

        internal void Generate(Type type)
        {
            Model.Text = type.ToString();

            Dictionary<string, TypeInfo> pFields = ReflectionHelper.GetPublicFields(type);
            Dictionary<string, TypeInfo> pProperty = ReflectionHelper.GetPublicProperty(type);
            Dictionary<string, TypeInfo> types = new();
            foreach(var item in pFields) types.Add(item.Key, item.Value);
            foreach(var item in pProperty) types.Add(item.Key, item.Value);

            List<Foldout> foldouts = new List<Foldout>();
            foreach(KeyValuePair<string, TypeInfo> item in types)
            {
                Foldout foldout = null;
                if (!foldouts.Any(e => e.text == item.Value.DeclaringType.Name))
                {
                    foldout = this.CreateFoldout(item.Value.DeclaringType.Name, true);
                    foldouts.Add(foldout);
                }
                else foldout = foldouts.First(e => e.text == item.Value.DeclaringType.Name);

                var result = AddPortByType(
                    ID: Guid.NewGuid().ToString(),
                    portText: $"{item.Key} ({item.Value.Type.Name})",
                    type: item.Value.Type,
                    value: DSUtilities.GetDefaultValue(item.Value.Type),
                    isInput: false,
                    isSingle: false,
                    isField: false,
                    cross: false,
                    portSide: PortSide.Output,
                    availableTypes: new Type[] { item.Value.Type },
                    isAnchorable: true
                    );
                foldout.Add(result.port);
            }
            RefreshExpandedState();
        }




        internal static Dictionary<string, Type> GetPublicFields(Type type)
        {
            Dictionary<string, Type> fieldDictionary = new Dictionary<string, Type>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
                fieldDictionary.Add(field.Name, field.FieldType);

            return fieldDictionary;
        }
        internal static Dictionary<string, Type> GetPublicProperties(Type type)
        {
            Dictionary<string, Type> propertyDictionary = new Dictionary<string, Type>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var property in properties)
                propertyDictionary.Add(property.Name, property.PropertyType);
            
            return propertyDictionary;
        }

        private static bool IsBackingFieldForProperty(string field) => field.StartsWith("<");



        public class TypeInfo
        {
            public Type Type { get; }
            public Type DeclaringType { get; }

            public TypeInfo(Type type, Type declaringType)
            {
                Type = type;
                DeclaringType = declaringType;
            }
        }

        public static class ReflectionHelper
        {
            public static Dictionary<string, TypeInfo> GetPublicFields(Type type)
            {
                Dictionary<string, TypeInfo> fieldDictionary = new Dictionary<string, TypeInfo>();
                PopulateFieldInfo(type, fieldDictionary);
                return fieldDictionary;
            }

            public static Dictionary<string, TypeInfo> GetPublicProperty(Type type)
            {
                Dictionary<string, TypeInfo> propertyDictionary = new Dictionary<string, TypeInfo>();
                PopulatePropertyInfo(type, propertyDictionary);
                return propertyDictionary;
            }

            private static void PopulateFieldInfo(Type type, Dictionary<string, TypeInfo> fieldDictionary)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var field in fields)
                {
                    Type fieldType = field.FieldType;
                    TypeInfo typeInfo = new TypeInfo(fieldType, type);

                    fieldDictionary.Add(field.Name, typeInfo);
                }

                Type baseType = type.BaseType;
                if (baseType != null && baseType != typeof(object))
                    PopulateFieldInfo(baseType, fieldDictionary);
            }

            private static void PopulatePropertyInfo(Type type, Dictionary<string, TypeInfo> propertyDictionary)
            {
                PropertyInfo[] propertyes = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var field in propertyes)
                {
                    Type fieldType = field.PropertyType;
                    TypeInfo typeInfo = new TypeInfo(fieldType, type);

                    propertyDictionary.Add(field.Name, typeInfo);
                }

                Type baseType = type.BaseType;
                if (baseType != null && baseType != typeof(object))
                    PopulatePropertyInfo(baseType, propertyDictionary);
            }
        }
    }
}
