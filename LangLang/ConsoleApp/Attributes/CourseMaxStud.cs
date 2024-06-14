using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ConsoleApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class CourseMaxStud:Attribute
    {
    }
}
