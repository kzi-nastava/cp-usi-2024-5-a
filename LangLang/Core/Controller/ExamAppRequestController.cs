using LangLang.Core.Model.DAO;
using System.Collections.Generic;
using LangLang.Core.Model;
using LangLang.Core.Observer;

namespace LangLang.Core.Controller
{
    public class ExamAppRequestController
    {
        private readonly ExamAppRequestDAO _appRequests;

        public ExamAppRequestController()
        {
            _appRequests = new ExamAppRequestDAO();
        }

        public List<ExamAppRequest> GetAll()
        {
            return _appRequests.GetAllAppRequests();
        }

        public void Add(ExamAppRequest appRequest)
        {
            _appRequests.Add(appRequest);
        }

        public void Delete(int id)
        {
            _appRequests.Remove(id);
        }

        
        public void Subscribe(IObserver observer)
        {
            _appRequests.Subscribe(observer);
        }

        public List<ExamAppRequest> GetStudentRequests(int studentId)
        {
            return _appRequests.GetStudentRequests(studentId);
        }

        public bool CancelRequest(ExamAppRequest appRequest, ExamSlotController examSlotController)
        {
            ExamSlot exam = examSlotController.GetById(appRequest.ExamSlotId);
            return _appRequests.CancelRequest(appRequest, exam);
        }

    }
}