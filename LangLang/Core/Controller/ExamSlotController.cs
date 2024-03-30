using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    public class ExamSlotController
    {
        private readonly ExamSlotsDAO _examSlots;

        public ExamSlotController()
        {
            _examSlots = new ExamSlotsDAO();
        }

        public Dictionary<int, ExamSlot> GetAllExamSlots()
        {
            return _examSlots.GetAllExamSlots();
        }

        public void Add(ExamSlot examSlot)
        {
            _examSlots.AddExamSlot(examSlot);
        }

        public void Delete(int examSlotId)
        {
            _examSlots.RemoveExamSlot(examSlotId);
        }

        public void Subscribe(IObserver observer)
        {
            _examSlots.Subscribe(observer);
        }

    }
}
