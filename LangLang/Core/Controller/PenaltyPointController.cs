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
        // Allows a tutor to assign a penalty point to a student for a specific course and deactivate the student's account if required
        public void GivePenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            _points.GivePenaltyPoint(student,tutor, course, appController);
        }

        //Returns list of penalty points of given student
        public List<PenaltyPoint> GetPenaltyPoints(Student student)
        {
            return _points.GetPenaltyPoints(student);
        }
        //Removes students oldest penalty point
        public void RemovePenaltyPoint(Student student)
        {
            _points.RemovePenaltyPoint(student);
        }

        public bool HasAlreadyGivenPenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            return _points.HasAlreadyGivenPenaltyPoint(student, tutor, course, appController);
        }
    }
}
