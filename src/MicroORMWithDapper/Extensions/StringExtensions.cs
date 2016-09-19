namespace MicroORMWithDapper
{
    using System;

    public static class StringExtensions
    {
        public static bool IsNotEmpty(this string content)
        {
            return !string.IsNullOrEmpty(content);
        }

        public static bool IsNotEmpty(this int value)
        {
            return value > 0;
        }

        public static Type GetEntityType<T>(this string className)
        {
            string name_space = "MicroORMWithDapper.";
            var objectType = Type.GetType(name_space + className);
            if (objectType == null)
            {
                objectType = typeof(T);
            }

            return objectType;
        }
    }
}