using LangLang.ConsoleApp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

public static class GenericForm
{
    public static T CreateEntity<T>() where T : class, new()
    {
        T entity = new T();
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (!Attribute.IsDefined(property, typeof(Show))) continue;

            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            string displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;
            Console.WriteLine($"Enter value for {displayName} ({property.PropertyType.Name}):");
            string input = Console.ReadLine();

            // Check if the property type is DateTime
            if (property.PropertyType == typeof(DateTime))
            {
                while (true)
                {
                    // Attempt to parse the input as DateTime
                    if (DateTime.TryParse(input, out DateTime dateTimeValue))
                    {
                        property.SetValue(entity, dateTimeValue);
                        break;
                    }
                    Console.WriteLine($"Invalid input. Please enter a valid DateTime for {property.Name}.");
                }
            }
            else
            {
                object value = Convert.ChangeType(input, property.PropertyType);
                property.SetValue(entity, value);
            }
        }

        return entity;
    }
    public static T UpdateEntity<T>(T entity) where T : class, new()
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (Attribute.IsDefined(property, typeof(AllowUpdate)))
            {
                // Get the current value of the property
                object currentValue = property.GetValue(entity);
                Console.WriteLine($"Current value for {property.Name}: {currentValue}");

                // Prompt the user to choose whether to update the property
                Console.WriteLine($"Do you want to update {property.Name}? (Y/N)");
                string choice = Console.ReadLine();

                if (choice.ToUpper() == "Y")
                {
                    Console.WriteLine($"Enter new value for {property.Name} ({property.PropertyType.Name}):");
                    string input = Console.ReadLine();
                    object value = Convert.ChangeType(input, property.PropertyType);
                    property.SetValue(entity, value);
                }
            }
        }

        return entity;
    }
}
