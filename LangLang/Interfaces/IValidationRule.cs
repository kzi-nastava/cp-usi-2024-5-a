using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Interfaces
{
    public interface IValidationRule<T>
    {
        string ErrorMessage { get; }
        bool Validate(T value);
    }
}
