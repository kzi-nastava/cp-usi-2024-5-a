using System;

namespace LangLang.ConsoleApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Show : Attribute
    {
        // You can add additional properties or methods if needed
    }
}