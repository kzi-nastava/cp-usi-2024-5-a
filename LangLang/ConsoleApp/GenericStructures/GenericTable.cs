using LangLang.BusinessLogic.UseCases;
using LangLang.ConsoleApp.Attributes;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LangLang.ConsoleApp.GenericStructures
{
    public class GenericTable<T>
    {
        private readonly List<T> entities;
        private readonly PropertyInfo[] properties;
        private bool isRoot;

        public GenericTable(bool root, T entity)
        {
            isRoot = isRoot;
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
        public GenericTable(IEnumerable<T> entities, bool root)
        {
            isRoot = root;
            this.entities = new();
            if (entities != null) this.entities = entities.ToList();
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
                    // Check if property type is a class and not string
                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        DisplayNestedHeaders(property);
                    }
                    else
                    {
                        Console.Write($"{displayName,-20} | ");
                    }
                }
            }
            if (isRoot) Console.WriteLine();
        }

        private void DisplayNestedHeaders(PropertyInfo property)
        {
            Type nestedType = property.PropertyType;
            var nestedTable = Activator.CreateInstance(typeof(GenericTable<>).MakeGenericType(nestedType), null, false);
            MethodInfo displayHeadersMethod = nestedTable.GetType().GetMethod("DisplayHeaders");
            displayHeadersMethod.Invoke(nestedTable, null);
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

                        // Check if property type is a class (and not string or other primitive)
                        if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        {
                            DisplayNestedContent(property, value);
                        }
                        else
                        {
                            Console.Write($"{value,-20} | ");
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
                Console.Write($"{"null",-20} | "); // Handle null values
                return;
            }

            var nestedType = property.PropertyType;
            var genericTableType = typeof(GenericTable<>).MakeGenericType(nestedType);
            var nestedTable = Activator.CreateInstance(genericTableType, new object[] { false, value });

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
        public T SelectRow()
        {
            Console.WriteLine("Select row (index starting from 1):");
            int index;
            if (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > entities.Count)
            {
                Console.WriteLine("Invalid selection.");
                return default;
            }
            return entities[index - 1];
        }
    }

}