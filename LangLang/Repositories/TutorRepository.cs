﻿using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private DatabaseContext _databaseContext {  get; set; }

        public TutorRepository(DatabaseContext context) {
            _databaseContext = context;
        }

        public void Add(Tutor tutor)
        {
            _databaseContext.Tutor.Add(tutor);
            _databaseContext.SaveChanges();
        }

        public void Deactivate(int id)
        {
            Tutor tutor = Get(id);
            if (tutor == null) return;

            _databaseContext.Tutor.Update(tutor);
            _databaseContext.SaveChanges();
        }

        public Tutor Get(int id)
        {
            return _databaseContext.Tutor.Find(id);
        }

        public List<Tutor> GetAll()
        {
            return _databaseContext.Tutor.ToList();
        }

        public List<Tutor> GetActive()
        {
            return _databaseContext.Tutor.Where(tutor => tutor.Profile.IsActive == true).ToList();
        }

        public void Update(Tutor tutor)
        {
            var existingTutor = _databaseContext.Tutor.Find(tutor.Id);
            if (existingTutor == null) return;
       
            _databaseContext.Entry(existingTutor).State = EntityState.Detached;
            _databaseContext.Tutor.Update(tutor);
            _databaseContext.SaveChanges();
        }


    }
}
