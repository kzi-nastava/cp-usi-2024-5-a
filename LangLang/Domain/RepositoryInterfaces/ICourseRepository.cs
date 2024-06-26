﻿using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    internal interface ICourseRepository
    {
        public List<Course> GetAll();
        public Course Get(int id);
        public void Add(Course course);
        public void Update(Course course);
        public void Delete(Course course);
    }
}
