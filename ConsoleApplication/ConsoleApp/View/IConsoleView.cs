using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.ConsoleApp.View
{
    public interface IConsoleView
    {
        void DisplayOutput(Profile user);
    }
}
