using System;

namespace ConsoleApplication.ConsoleApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowUpdate : Attribute
    {
    }
}
