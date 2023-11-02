using System;

namespace DialogueSystem.Utilities
{
    public static class DSConstants
    {
        public readonly static Type[] AvalilableTypes;
        public readonly static Type[] NumberTypes;
        public readonly static Type[] DialogueTypes;

        public readonly static string All;
        public readonly static string Number;
        public readonly static string Dialogue;
        public readonly static string Int;
        public readonly static string String;
        public readonly static string Float;
        public readonly static string Double;
        public readonly static string Bool;

        static DSConstants()
        {
            AvalilableTypes = new Type[]
            {
                typeof(string),
                typeof(int),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(float),
                typeof(Single),
                typeof(double),
                typeof(bool),
                typeof(Dialogue),
            };
            NumberTypes = new Type[]
            {
                typeof(int),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(float),
                typeof(Single),
                typeof(double),
            };
            DialogueTypes = new Type[]
            {
                typeof(Dialogue),
            };

            All = "All";
            Int = typeof(int).Name;
            String = typeof(string).Name;
            Float = typeof(float).Name;
            Double = typeof(double).Name;
            Bool = typeof(bool).Name;
            Number = "Number";
            Dialogue = "Dialogue";
        }
    }
}