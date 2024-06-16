using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReferenceAttribute : Attribute
    {
        public Type ReferencedType { get; }

        public ReferenceAttribute(Type referencedType)
        {
            ReferencedType = referencedType;
        }
    }
}
