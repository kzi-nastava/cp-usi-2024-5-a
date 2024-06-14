using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IExamSlotRepository
    {
        public List<ExamSlot> GetAll();
        public ExamSlot Get(int id);
        public List<ExamSlot> GetByTutor(Tutor tutor);
        public void Add(ExamSlot exam);
        public void Delete(ExamSlot exam);
        public void Update(ExamSlot exam);
    }
}