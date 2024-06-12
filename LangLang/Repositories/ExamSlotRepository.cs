using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    public class ExamSlotRepository : IExamSlotRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ExamSlotRepository(DatabaseContext context)
        {
            _databaseContext = context;
        }

        public ExamSlot? Get(int id)
        {
            return _databaseContext.ExamSlot.Find(id);
        }

        public List<ExamSlot> GetAll()
        {
            return _databaseContext.ExamSlot.ToList();
        }

        public List<ExamSlot> GetExams(Tutor tutor)
        {
            return _databaseContext.ExamSlot.Where(es => es.TutorId == tutor.Id).ToList();
        }

        public void Add(ExamSlot exam)
        {
            _databaseContext.ExamSlot.Add(exam);
            _databaseContext.SaveChanges();

        }

        public void Update(ExamSlot exam)
        {
            _databaseContext.ExamSlot.Update(exam);
            _databaseContext.SaveChanges();
        }

        public void Delete(ExamSlot exam)
        {
            _databaseContext.ExamSlot.Remove(exam);
            _databaseContext.SaveChanges();
        }
    }
}