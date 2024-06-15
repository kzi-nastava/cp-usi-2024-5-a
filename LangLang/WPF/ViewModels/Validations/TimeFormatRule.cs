using LangLang.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LangLang.WPF.ViewModels.Validations
{
    public class TimeFormatRule : IValidationRule<string>
    {
        private readonly Regex _timeRegex = new("^(?:[01]\\d|2[0-3]):(?:[0-5]\\d)$");
        public string ErrorMessage => "Time must be of format HH:mm.";

        public bool Validate(string value)
        {
            return _timeRegex.Match(value).Success;
        }
    }
}
