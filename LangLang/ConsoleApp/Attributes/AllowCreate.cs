﻿using System;

namespace LangLang.ConsoleApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowCreate : Attribute
    {
    }
}
