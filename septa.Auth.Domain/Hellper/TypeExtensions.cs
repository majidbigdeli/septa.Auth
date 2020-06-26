using System;
using System.Collections.Generic;

namespace septa.Auth.Domain.Hellper
{
    public static class TypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            Check.NotNull<Type>(type, nameof(type));
            return type.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            Check.NotNull<Type>(type, nameof(type));
            Check.NotNull<Type>(targetType, nameof(targetType));
            return targetType.IsAssignableFrom(type);
        }

        public static Type[] GetBaseClasses(this Type type, bool includeObject = true)
        {
            Check.NotNull<Type>(type, nameof(type));
            List<Type> types = new List<Type>();
            TypeExtensions.AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        private static void AddTypeAndBaseTypesRecursively(
          List<Type> types,
          Type type,
          bool includeObject)
        {
            Check.NotNull<List<Type>>(types, nameof(types));
            if (type == (Type)null || !includeObject && type == typeof(object))
                return;
            TypeExtensions.AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            types.Add(type);
        }
    }

}
