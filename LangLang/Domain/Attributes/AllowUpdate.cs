﻿using System;

namespace LangLang.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowUpdate : Attribute
    {
    }
}