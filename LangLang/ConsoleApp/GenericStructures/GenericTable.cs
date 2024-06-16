using LangLang.ConsoleApp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;

namespace LangLang.ConsoleApp.GenericStructures
{
    public class GenericTable<T>
    {
        private readonly List<T> entities;
        private readonly PropertyInfo[] properties;
        private bool isRoot;

        public GenericTable(T entity, bool isRoot)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entities = new List<T> { entity };
            properties = typeof(T).GetProperties();
            this.isRoot = isRoot;
        }

        public GenericTable()
        {
            isRoot = false;
            entities = new List<T>();
            properties = typeof(T).GetProperties();
        }

        public GenericTable(IEnumerable<T> entities, bool isRoot)
        {
            this.isRoot = isRoot;
            this.entities = entities?.ToList() ?? new List<T>();
            properties = typeof(T).GetProperties();
        }

        public void DisplayTable()
        {
            DisplayHeaders();
            DisplayContent();
        }

        public void DisplayHeaders()
        {
            if (isRoot) Console.Write($"{"Row",-5} | ");
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(Show)))
                {
                    var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                    string displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;

                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        // If it's a list, just print the display name
                        Console.Write($"{displayName,-25} | ");
                    }
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        // For nested classes
                        DisplayNestedHeaders(property);
                    }
                    else
                    {
                        // For simple properties
                        Console.Write($"{displayName,-25} | ");
                    }
                }
            }
            if (isRoot) Console.WriteLine();
        }

        private void DisplayNestedHeaders(PropertyInfo property)
        {
            Type nestedType = property.PropertyType;
            var nestedTable = Activator.CreateInstance(typeof(GenericTable<>).MakeGenericType(nestedType), false);
            MethodInfo displayHeadersMethod = nestedTable.GetType().GetMethod("DisplayHeaders");
            displayHeadersMethod?.Invoke(nestedTable, null);
        }

        public void DisplayContent()
        {
            int rowNumber = 1;
            foreach (var entity in entities)
            {
                if (isRoot) Console.Write($"{rowNumber++,-5} | ");
                foreach (var property in properties)
                {
                    if (Attribute.IsDefined(property, typeof(Show)))
                    {
                        var value = property.GetValue(entity);

                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            var listType = value.GetType().GetGenericArguments()[0];
                            if (listType.IsEnum || listType == typeof(string))
                            {
                                // Display enum list directly
                                var list = (System.Collections.IList)value;
                                string enumValues = string.Join(", ", list.Cast<object>());
                                Console.Write($"{enumValues,-25} | ");
                                continue;
                            }
                            var nestedList = (System.Collections.IList)value;
                            foreach (var item in nestedList)
                            {
                                DisplayNestedContent(property, item);
                            }

                        }
                        else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        {
                            // Handle nested class
                            DisplayNestedContent(property, value);
                        }
                        else
                        {
                            // Handle simple property
                            Console.Write($"{value,-25} | ");
                        }
                    }
                }
                if (isRoot) Console.WriteLine();
            }
        }

        private void DisplayNestedContent(PropertyInfo property, object value)
        {
            if (value == null)
            {
                Console.Write($"{"null",-25} | ");
                return;
            }

            var nestedType = value.GetType();

            try
            {
                if (nestedType.IsGenericType && nestedType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Handle nested list
                    var listType = nestedType.GetGenericArguments()[0];
                    var list = (System.Collections.IList)value;

                    if (listType.IsEnum)
                    {
                        // Print enums directly from list
                        Console.Write($"{string.Join(", ", list.Cast<object>())}, ");
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            DisplayNestedContent(property, item);
                        }
                    }
                }
                else if (nestedType.IsEnum)
                {
                    // Print enum directly
                    Console.Write($"{value,-25} | ");
                }
                else
                {
                    // Handle nested object
                    var genericTableType = typeof(GenericTable<>).MakeGenericType(nestedType);
                    var nestedTable = Activator.CreateInstance(genericTableType, new object[] { value, false });
                    var displayContentMethod = genericTableType.GetMethod("DisplayContent");

                    if (displayContentMethod != null)
                    {
                        displayContentMethod.Invoke(nestedTable, null);
                    }
                    else
                    {
                        Console.WriteLine($"Method 'DisplayContent' not found on type {genericTableType.Name}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public T SelectRow()
        {
            Console.WriteLine("Select row (index starting from 1):");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > entities.Count)
            {
                Console.WriteLine("Invalid selection.");
                return default;
            }
            return entities[index - 1];
        }
    }
}
