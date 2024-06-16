using System;

namespace ConsoleApplication.ConsoleApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowCreate : Attribute
    {
    }
}
