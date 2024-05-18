﻿using System;
using LangLang.Core.Repository.Serialization;
using LangLang.Domain.Models;

namespace LangLang.Core.Model
{
    public class Director : ISerializable, IProfileHolder
    {
        public Profile Profile{ get; set; }
        public int Id => Profile.Id;

        public Director(int id, string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isActive) {
            Profile = new (id, name, lastName, gender, birthDate, phoneNumber, email, password, role, isActive);
        }

        public Director() { }

        public string[] ToCSV()
        {
            return new string[]{ 
                Profile.ToString()};
        }

        public void FromCSV(string[] values)
        {
            Profile = new (values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9]);
        }

    }
}
