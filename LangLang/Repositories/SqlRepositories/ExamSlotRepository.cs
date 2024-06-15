using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories.SqlRepositories
{
    public class ExamSlotRepository : IExamSlotRepository
    {
        private readonly DatabaseContext _context;

        public ExamSlotRepository(DatabaseContext context)
        {
            _context = context;
        }

        public ExamSlot? Get(int id)
        {
            return _context.ExamSlot.Find(id);
        }

        public List<ExamSlot> GetAll()
        {
            return _context.ExamSlot.ToList();
        }

        public List<ExamSlot> GetByTutor(Tutor tutor)
        {
            return _context.ExamSlot.Where(es => es.TutorId == tutor.Id).ToList();
        }

        public void Add(ExamSlot exam)
        {
            _context.ExamSlot.Add(exam);
            _context.SaveChanges();
        }

        public void Update(ExamSlot exam)
        {
            _context.ExamSlot.Update(exam);
            _context.SaveChanges();
        }

        public void Delete(ExamSlot exam)
        {
            _context.ExamSlot.Remove(exam);
            _context.SaveChanges();
        }
    }
}