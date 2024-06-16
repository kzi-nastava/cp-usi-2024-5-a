using LangLang.Domain.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace ConsoleApplication.ConsoleApp.GenericStructures
{
    public static class GenericForm
    {
        public static T CreateEntity<T>() where T : class, new()
        {
            T entity = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(AllowCreate))) continue;

                var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                string displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;
                Type pType = property.PropertyType;
                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Console.WriteLine($"Enter values for {displayName} (press Enter to finish):");
                    Type elementType = pType.GetGenericArguments()[0];
                    IList list = (IList)Activator.CreateInstance(pType);
                    bool first = true;
                    while (true)
                    {
                        Console.Write($"Enter {elementType.Name} value: ");
                        string input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input) && first == false) break;

                        if (TryParseInput(elementType, input, out object elementValue))
                        {
                            if (first) { first = false; }
                            list.Add(elementValue);
                            continue;
                        }
                        if (first)
                        {
                            Console.WriteLine($"Must enter at least one valid element for {elementType.Name}.");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid input. Please enter a valid {elementType.Name}.");
                        }
                    }

                    property.SetValue(entity, list);
                    continue;
                }
                else if (pType.IsClass && pType != typeof(string))
                {
                    Console.WriteLine($"Creating a new instance for {property.Name}:");
                    var nestedEntity = CreateEntityForType(pType);
                    property.SetValue(entity, nestedEntity);
                    continue;
                }
                while (true)
                {
                    Console.WriteLine($"Enter value for {displayName} ({property.PropertyType.Name}):");
                    string input = Console.ReadLine();
                    if(TryParseInput(property.PropertyType, input, out object value))
                    {
                        property.SetValue(entity, value);
                        break;
                    }
                }
            }

            return entity;
        }
        
        public static object CreateEntityForType(Type type)
        {
            var method = typeof(GenericForm).GetMethod(nameof(CreateEntity), BindingFlags.Public | BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(type);
            return genericMethod.Invoke(null, null);
        }
        public static object UpdateEntityForType(Type type, object entity)
        {
            var method = typeof(GenericForm).GetMethod(nameof(UpdateEntity), BindingFlags.Public | BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(type);
            return genericMethod.Invoke(null, new object[] { entity });
        }
        public static T UpdateEntity<T>(T entity) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                
                if (Attribute.IsDefined(property, typeof(AllowUpdate)))
                {
                    object currentValue = property.GetValue(entity);
                    var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                    string displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;
                    
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var list = (System.Collections.IList)currentValue;
                        string enumValues = string.Join(", ", list.Cast<object>());
                        currentValue = enumValues;
                    }
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        Console.WriteLine($"Updating {property.Name}:");
                        var nestedEntity = UpdateEntityForType(property.PropertyType, currentValue);
                        property.SetValue(entity, nestedEntity);
                        continue;
                    }
                    Console.WriteLine($"Current value for {displayName}: {currentValue}");

                    Console.WriteLine($"Do you want to update {displayName}? (Y/N)");
                    string choice = Console.ReadLine();

                    if (choice.ToUpper() == "Y")
                    {
                        Type pType = property.PropertyType;
                        if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            Console.WriteLine($"Enter values for {displayName} (press Enter to finish):");
                            Type elementType = pType.GetGenericArguments()[0];
                            IList list = (IList)Activator.CreateInstance(pType);

                            bool first = true;
                            while (true)
                            {
                                Console.Write($"Enter {elementType.Name} value: ");
                                string input = Console.ReadLine();
                                if (string.IsNullOrEmpty(input) && first == false) break;

                                if (TryParseInput(elementType, input, out object elementValue))
                                {
                                    if (first) { first = false; }
                                    list.Add(elementValue);
                                    continue;
                                }
                                if (first)
                                {
                                    Console.WriteLine($"Must enter at least one valid element for {elementType.Name}.");
                                }
                                else
                                {
                                    Console.WriteLine($"Invalid input. Please enter a valid {elementType.Name}.");
                                }
                            }

                            property.SetValue(entity, list);
                            continue;
                        }
                        else if (pType.IsClass && pType != typeof(string))
                        {
                            Console.WriteLine($"Creating a new instance for {property.Name}:");
                            var nestedEntity = CreateEntityForType(pType);
                            property.SetValue(entity, nestedEntity);
                            continue;
                        }
                        while (true)
                        {
                            Console.WriteLine($"Enter value for {displayName} ({property.PropertyType.Name}):");
                            string valueInput = Console.ReadLine();
                            if (TryParseInput(property.PropertyType, valueInput, out object value))
                            {
                                property.SetValue(entity, value);
                                break;
                            }
                        }
                    }
                }
            }

            return entity;
        }
        private static bool TryParseInput(Type elementType, string input, out object result)
        {
            result = null;
            if (elementType == typeof(DateTime))
            {
                string format = "HH:mm dd.MM.yyyy";
                if (DateTime.TryParseExact(input, format, null, System.Globalization.DateTimeStyles.None, out DateTime dateTimeValue))
                {
                    result = dateTimeValue;
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid DateTime in format HH:mm dd.MM.yyyy");
                return false;
            }
            else if (elementType == typeof(string))
            {
                result = input;
                return true;
            }
            else if (elementType == typeof(int))
            {
                if (int.TryParse(input, out int intValue))
                {
                    if (intValue <= 0)
                    {
                        Console.WriteLine("The integer value must be greater than 0.");
                        return false;
                    }
                    result = intValue;
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid integer.");
                return false;
            }
            else if (elementType == typeof(bool))
            {
                if (bool.TryParse(input, out bool boolValue))
                {
                    result = boolValue;
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter 'true' or 'false'.");
                return false;
            }
            else if (elementType.IsEnum)
            {
                if (Enum.TryParse(elementType, input, true, out object enumValue))
                {
                    result = enumValue;
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid value from the options: {string.Join(", ", Enum.GetNames(elementType))}.");
                return false;

            }
            else if (elementType == typeof(double))
            {
                if (double.TryParse(input, out double doubleValue))
                {
                    result = doubleValue;
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid double.");
                return false;
            }
            else
            {
                try
                {
                    object value = Convert.ChangeType(input, elementType);
                    return true;
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine($"Invalid input. Cannot convert {input} to {elementType.Name}.");
                    return false;
                }
            }
        }
    }
}

