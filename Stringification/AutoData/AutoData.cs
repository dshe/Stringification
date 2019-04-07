using NodaTime;
using StringEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

// AutoFixture is not used because it:
//   cannot set internal or private properties
//   has a confusing API
//   cannot create observables
//   would add another dependency
// Bogus is also popular

#nullable enable

namespace Stringification
{
    public static class AutoData
    {
        private static readonly Random Rand = new Random();

        public static T Create<T>() => (T)Create(typeof(T));

        public static List<T> Create<T>(int count)
        {
            var list = new List<T>();
            for (var i = 0; i < count; i++)
            {
                T instance = Create<T>();
                list.Add(instance);
            }
            return list;
        }

        public static object Create(Type type)
        {
            var utype = Nullable.GetUnderlyingType(type);
            if (utype != null)
                type = utype;

            if (type == typeof(char))
                return Guid.NewGuid().ToString("N")[0];
            if (type == typeof(string))
                return Guid.NewGuid().ToString("N"); // 32 character hex
            if (type == typeof(bool))
                return true;
            if (type == typeof(int))
                return Rand.Next(100, 1000);
            if (type == typeof(long))
                return (long)Rand.Next(1000, 10000);
            if (type == typeof(float))
                return (float)Rand.NextDouble() * 10;
            if (type == typeof(double))
                return Rand.NextDouble() * 100;
            if (type == typeof(decimal))
                return (decimal)Rand.NextDouble() * 1000;
            if (type == typeof(DateTime))
                return DateTime.UtcNow;
            if (type == typeof(LocalDateTime))
                return SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).LocalDateTime;
            if (type == typeof(ZonedDateTime))
                return SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault());

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                var values = typeInfo.GetEnumValues();
                var i = Rand.Next(1, values.Length);
                return values.GetValue(i);
            }

            if (typeInfo.IsStringEnum())
            {
                var method = typeInfo.BaseType.GetMethod("ToStringEnums");
                var fields = (IList)method.Invoke(null, null);
                int i = Rand.Next(1, fields.Count);
                return fields[i];
            }

            if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo))
            {
                var genericTypes = typeInfo.GenericTypeArguments;
                var listType = typeof(List<>).MakeGenericType(genericTypes).GetTypeInfo();
                var list = Utilities.CreateInstance(listType);
                AddItemsToList(list);
                return list;
            }

            if (typeInfo.IsClass)
            {
                var classInstance = Utilities.CreateInstance(typeInfo);
                SetClassProperties(classInstance);
                return classInstance;
            }

            throw new InvalidDataException($"Cannot create type '{typeInfo.Name}'.");
        }

        private static void SetClassProperties(object classInstance)
        {
            var type = classInstance.GetType().GetTypeInfo();
            Debug.Assert(type.IsClass);

            foreach (var property in type.DeclaredProperties)
            {
                var propertyType = property.PropertyType.GetTypeInfo();
                var propertyValue = property.GetValue(classInstance);
                var defaultPropertyValue = DefaultValueOfType(propertyType);

                if (Equals(propertyValue, defaultPropertyValue))
                {
                    try
                    {
                        var value = Create(propertyType);
                        if (property.CanWrite)
                            property.SetValue(classInstance, value, BindingFlags.CreateInstance | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
                        else // search for private backing field of public property
                        {
                            var fieldName = "<" + property.Name + ">";
                            var field = type.DeclaredFields.Where(f => f.Name.StartsWith(fieldName)).SingleOrDefault();
                            field.SetValue(classInstance, value, BindingFlags.CreateInstance | BindingFlags.NonPublic, null, CultureInfo.InvariantCulture);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Could not set property on: {type.Name}.{property.Name}", e);
                    }
                }
                else if (propertyValue is IEnumerable enumerable && !(propertyValue is string))
                {
                    if (!enumerable.GetEnumerator().MoveNext())
                        AddItemsToList(enumerable);
                }
                else if (propertyType.IsClass)
                    SetClassProperties(propertyValue);
            }
        }

        private static void AddItemsToList(object obj)
        {
            var enumerable = (IEnumerable)obj;
            var type = enumerable.GetType();
            var genericTypes = type.GenericTypeArguments;
            var listType = typeof(List<>).MakeGenericType(genericTypes);
            var addMethod = listType.GetMethod("Add", genericTypes);
            if (addMethod == null)
                throw new Exception("Cannot find Add method.");
            // use the Add method to add 3 items to the list
            for (var i = 0; i < 3; i++)
            {
                var item = Create(genericTypes[0]);
                addMethod.Invoke(enumerable, new[] { item });
            }
        }

        private static object? DefaultValueOfType(TypeInfo type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var utype = Nullable.GetUnderlyingType(type);
            if (utype != null)
                return null;

            if (type == typeof(string))
                return "";

            if (type.IsValueType)
                return Utilities.CreateInstance(type);

            return null; // reference type, unknown
        }
    }
}
