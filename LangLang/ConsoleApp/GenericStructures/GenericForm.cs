using LangLang.ConsoleApp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace LangLang.ConsoleApp.GenericStructures
{
    public static class GenericForm
    {
        private static bool isCourseOnline { get; set; }
        public static T CreateEntity<T>() where T : class, new()
        {
            T entity = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(AllowCreate))) continue;

                if (Attribute.IsDefined(property, typeof(CourseDays)))
                {
                    List<DayOfWeek> days = inputDays();
                    property.SetValue(entity, days);
                    continue;
                }

                if (Attribute.IsDefined(property, typeof(CourseMaxStud)))
                {
                    if (isCourseOnline)
                    {
                        continue;
                    }
                }

                var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                string displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;
                Type pType = property.PropertyType;
                if (pType.IsClass && pType != typeof(string))
                {
                    Console.WriteLine($"Creating a new instance for {property.Name}:");
                    var nestedEntity = CreateEntityForType(pType);
                    property.SetValue(entity, nestedEntity);
                    continue;
                }
                bool success = false;
                while ( !success )
                {
                    Console.WriteLine($"Enter value for {displayName} ({property.PropertyType.Name}):");
                    string input = Console.ReadLine();
                    success = CheckInput(property, input, entity);

                }
            }

            return entity;
        }
        private static bool CheckInput<T>(PropertyInfo property, string input, T entity) {
            Type pType = property.PropertyType;
            bool courseOnline = false;
            if(Attribute.IsDefined(property, typeof(CourseOnline)))
            {
                courseOnline = true;
            }
            // Check if the property type is DateTime
            if (pType == typeof(DateTime))
            {
                string format = "HH:mm dd.MM.yyyy";
                // Attempt to parse the input as DateTime
                if (DateTime.TryParseExact(input, format, null, System.Globalization.DateTimeStyles.None, out DateTime dateTimeValue))
                {
                    if (dateTimeValue <= DateTime.Now)
                    {
                        Console.WriteLine("The date and time are not in the future.");
                        return false;
                    }
                    property.SetValue(entity, dateTimeValue);
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid DateTime for {property.Name} in format HH:mm dd.MM.yyyy");
                return false;

            }
            else if (pType == typeof(string))
            {
                // Set the string value directly
                property.SetValue(entity, input);
                return true;
            }
            else if (pType == typeof(int))
            {
                if (int.TryParse(input, out int intValue))
                {
                    property.SetValue(entity, intValue);
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid integer for {property.Name}.");
                return false;
            }
            else if (pType == typeof(bool))
            {
                if (bool.TryParse(input, out bool boolValue))
                {
                    isCourseOnline = boolValue;
                    property.SetValue(entity, boolValue);
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter 'true' or 'false' for {property.Name}.");
                return false;
            }
            else if (pType.IsEnum)
            {
                if (Enum.TryParse(pType, input, true, out object enumValue))
                {
                    property.SetValue(entity, enumValue);
                    return true;
                }
                Console.WriteLine($"Invalid input. Please enter a valid value for {property.Name} from the options: {string.Join(", ", Enum.GetNames(pType))}.");
                return false;

            }
            else
            {
                try
                {
                    object value = Convert.ChangeType(input, property.PropertyType);
                    property.SetValue(entity, value);
                    return true;
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine($"Invalid input. Cannot convert {input} to {property.PropertyType.Name} for {property.Name}.");
                    return false;
                }
            }
        }
        public static object CreateEntityForType(Type type)
        {
            var method = typeof(GenericForm).GetMethod(nameof(CreateEntity), BindingFlags.Public | BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(type);
            return genericMethod.Invoke(null, null);
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
        private static List<DayOfWeek> inputDays()
        {
            Console.WriteLine($"Input the number of days on which the course will be held (between 1 and 5):");
            string input = Console.ReadLine();
            while (true)
            {
                switch (input)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        return selectedDays(int.Parse(input));
                    case "5":
                        Console.WriteLine("You have selected all of the days");
                        return new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
                    default:
                        Console.WriteLine("Invalid choice try again.");
                        break;
                }
            }
        }
        private static List<DayOfWeek> selectedDays(int num)
        {
            List<DayOfWeek> result = new List<DayOfWeek>();
            while (num != 0)
            {
                num -= 1;
                Console.WriteLine("Input day of the week:");
                string input = Console.ReadLine();
                while (true)
                {
                    bool success = Enum.TryParse<DayOfWeek>(input, true, out DayOfWeek day);
                    if (success && result.Contains(day) == false)
                    {
                        result.Add(day);
                        break;
                    }
                    if (success)
                    {
                        Console.WriteLine("You have already selected that day. Try again: ");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again: ");
                    }
                    input = Console.ReadLine();
                }
            }
            return result;
        }
    }
}

