
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IWithdrawalRequestRepository
    {
        public List<WithdrawalRequest> GetAll();
        public WithdrawalRequest Get(int id);
        public void Add(WithdrawalRequest request);
        public void Update(WithdrawalRequest request);
        public void Delete(int id);
        public List<WithdrawalRequest> GetByStudent(Student student);
        public List<WithdrawalRequest> GetByCourse(Course course);
        public void Save();
        public Dictionary<int, WithdrawalRequest> Load();
        public void Subscribe(IObserver observer);
    }
}
