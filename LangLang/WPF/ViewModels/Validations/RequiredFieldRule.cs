using LangLang.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.WPF.ViewModels.Validations
{
    public class RequiredFieldRule<T> : IValidationRule<T>
    {
        public string ErrorMessage { get; }

        public RequiredFieldRule(string errorMessage = "This field is required.")
        {
            ErrorMessage = errorMessage;
        }

        public bool Validate(T value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is string stringValue)
            {
                return !string.IsNullOrEmpty(stringValue);
            }

            return true;
        }
    }

}
