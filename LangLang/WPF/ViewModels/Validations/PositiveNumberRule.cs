using LangLang.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.WPF.ViewModels.Validations
{
    public class PositiveNumberRule : IValidationRule<string>
    {
        public string ErrorMessage => "Must input a positive integer.";

        public bool Validate(string value)
        {
            return int.TryParse(value, out int result) && result > 0;
        }
    }
}
