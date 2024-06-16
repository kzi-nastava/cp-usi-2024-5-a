using System;

namespace ConsoleApp1.ConsoleApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowCreate : Attribute
    {
    }
}
