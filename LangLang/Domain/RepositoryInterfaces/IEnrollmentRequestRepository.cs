using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IEnrollmentRequestRepository
    {
        public List<EnrollmentRequest> GetAll();
        public EnrollmentRequest Get(int id);
        public void Add(EnrollmentRequest request);
        public void Update(EnrollmentRequest request);
        public void Delete(int id);
        public List<EnrollmentRequest> GetByStudent(Student student);
        public List<EnrollmentRequest> GetByCourse(Course course);
        public void Cancel(int id);
        public void Pause(EnrollmentRequest request);
        public void Accept(EnrollmentRequest request);
        public void SetPending(EnrollmentRequest request);
        public void Save();
        public Dictionary<int, EnrollmentRequest> Load();
        public void Subscribe(IObserver observer);
    }
}
