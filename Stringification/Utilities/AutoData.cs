using NodaTime;
using StringEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

// AutoFixture is not used because it:
//   cannot set internal or private properties
//   has a confusing API
//   cannot create observables
// (Bogus is also popular)

namespace Stringification
{
    public static class AutoData
    {
        private static readonly Random Rand = new Random();

        public static T Create<T>()
        {
            var type = typeof(T);
            var instance = Create(type);
            if (instance == null)
                throw new InvalidDataException($"Cannot create type '{type.Name}'.");
            return (T)instance;
        }

        public static IList<T> Create<T>(int count) =>
           Enumerable.Range(0, count).Select(_ => Create<T>()).ToList();

        public static object? Create(Type type)
        {
            var utype = Nullable.GetUnderlyingType(type);
            if (utype != null)
                type = utype;

            if (type == typeof(char))
                return GetRandomString(1)[0];
            if (type == typeof(string))
                return GetRandomString(8);
            if (type == typeof(bool))
                return true;
            if (type == typeof(int))
                return Rand.Next(10, 100);
            if (type == typeof(long))
                return (long)Rand.Next(100, 1000);
            if (type == typeof(float))
                return (float)Math.Round(Rand.NextDouble() * 10, 2);
            if (type == typeof(double))
                return Math.Round(Rand.NextDouble() * 100, 2);
            if (type == typeof(decimal))
                return (decimal)Math.Round(Rand.NextDouble() * 1000, 2);
            if (type == typeof(DateTime))
                return DateTime.UtcNow;
            if (type == typeof(Instant))
                return SystemClock.Instance.GetCurrentInstant();
            if (type == typeof(LocalTime))
                return SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).TimeOfDay;
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
                var fields = typeInfo.DeclaredFields.ToList();
                var i = Rand.Next(1, fields.Count);
                return fields[i].GetValue(null);
            }

            if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo))
            {
                var genericTypes = typeInfo.GenericTypeArguments;
                if (genericTypes.Length == 1) // list
                {
                    var listType = typeof(List<>).MakeGenericType(genericTypes).GetTypeInfo();
                    var list = Utilities.CreateInstance(listType);
                    AddItemsToList(list);
                    return list;
                }
                if (genericTypes.Length == 2) // dictionary
                {
                    var dictType = typeof(Dictionary<,>).MakeGenericType(genericTypes).GetTypeInfo();
                    var dict = Utilities.CreateInstance(dictType);
                    AddItemsToDictionary(dict);
                    return dict;
                }
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                var genericTypes = type.GetGenericArguments();
                var value1 = Create(genericTypes[0]);
                var value2 = Create(genericTypes[1]);
                var ctor = type.GetConstructor(genericTypes);
                var result = ctor.Invoke(new[] { value1, value2 });
                return result;
            }

            if (typeInfo.IsClass)
            {
                if (typeInfo.IsAbstract)
                    return null;
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

                if (propertyValue == null || Equals(propertyValue, defaultPropertyValue))
                {
                    try
                    {
                        var value = Create(propertyType);
                        if (value == null)
                            continue;
                        if (property.CanWrite)
                            property.SetValue(classInstance, value);
                        else // search for private backing field of public property
                        {
                            var fieldName = "<" + property.Name + ">";
                            type.DeclaredFields
                                .Where(f => f.Name.StartsWith(fieldName))
                                .SingleOrDefault()
                                .SetValue(classInstance, value);
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
                else if (propertyType.IsClass && propertyType != typeof(string))
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
                if (item == null)
                    return;
                addMethod.Invoke(enumerable, new[] { item });
            }
        }

        private static void AddItemsToDictionary(object obj)
        {
            var dict = (IDictionary)obj;
            var type = dict.GetType();
            var genericTypes = type.GenericTypeArguments;
            var listType = typeof(Dictionary<,>).MakeGenericType(genericTypes);
            var addMethod = listType.GetMethod("Add", genericTypes);
            if (addMethod == null)
                throw new Exception("Cannot find Add method.");
            // use the Add method to add 3 items to the list
            for (var i = 0; i < 3; i++)
            {
                var key = Create(genericTypes[0]);
                var value = Create(genericTypes[1]);
                if (key == null || value == null)
                    return;
                addMethod.Invoke(dict, new[] { key, value });
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

        private static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];

            for (int i = 0; i < length; i++)
                stringChars[i] = chars[Rand.Next(chars.Length)];

            return new string(stringChars);
        }
    }
}
