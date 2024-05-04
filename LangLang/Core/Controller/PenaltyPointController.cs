using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    public class PenaltyPointController
    {
        private readonly PenaltyPointDAO _points;

        public PenaltyPointController()
        {
            _points = new PenaltyPointDAO();
        }

        public PenaltyPoint? GetById(int id)
        {
            return _points.GetById(id);
        }
        public List<PenaltyPoint> GetAllPoints()
        {
            return _points.GetAllPoints().Values.ToList();
        }

        public void GivePenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            _points.GivePenaltyPoint(student,tutor, course, appController);
        }

        //Returns list of penalty points of given student
        public List<PenaltyPoint> GetPenaltyPoints(Student student)
        {
            return _points.GetPenaltyPoints(student);
        }
        public void RemovePenaltyPoint(Student student, AppController appController)
        {
            _points.RemovePenaltyPoint(student, appController);
        }

    }
}
