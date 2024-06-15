using LangLang.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.WPF.ViewModels.Validations
{
    public class FutureDateRule : IValidationRule<DateTime>
    {
        public string ErrorMessage => "Please enter a future date.";

        public bool Validate(DateTime value)
        {
            return value > DateTime.Now;
        }
    }
}
