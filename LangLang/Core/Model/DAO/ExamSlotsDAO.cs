using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Core.Observer;
using System.Collections;
using System.Windows.Input;


namespace LangLang.Core.Model.DAO
{
    public class ExamSlotsDAO : Subject
    {
        private readonly Dictionary<int, ExamSlot> _examSlots;
        private readonly Repository<ExamSlot> _repository;

        public ExamSlotsDAO()
        {
            _repository = new Repository<ExamSlot>("examSlots.csv");
            _examSlots = _repository.Load();
        }
        private int GenerateId()
        {
            if (_examSlots.Count == 0) return 0;
            return _examSlots.Keys.Max() + 1;
        }

        private ExamSlot? GetExamSlotById(int id)
        {
            return _examSlots[id];
        }

        public List<ExamSlot> GetAllExams()
        {
            return _examSlots.Values.ToList();
        }


        //function takes examslot and adds it to dictionary of examslots
        //function saves changes and returns added examslot
        public ExamSlot AddExamSlot(ExamSlot examSlot)
        {
            examSlot.Id = GenerateId();
            _examSlots[examSlot.Id] = examSlot;
            _repository.Save(_examSlots);
            NotifyObservers();
            return examSlot;
        }

        //function takes id of examslot and removes examslot with that id
        //function saves changes and returns removed examslot
        public ExamSlot RemoveExamSlot(int id)
        {
            ExamSlot examSlot = GetExamSlotById(id);
            if (examSlot == null) return null;

            _examSlots.Remove(id);
            _repository.Save(_examSlots);
            NotifyObservers();
            return examSlot;
        }

        //function for updating examslot takes new version of examslot and updates existing examslot to be same as new one
        //function saves changes and returns updated examslot
        public ExamSlot UpdateExamSlot(ExamSlot examSlot)
        {
            ExamSlot oldExamSlot = GetExamSlotById(examSlot.Id);
            if (oldExamSlot == null) return null;

            oldExamSlot.TutorId = examSlot.TutorId;
            oldExamSlot.MaxStudents = examSlot.MaxStudents;
            oldExamSlot.TimeSlot = examSlot.TimeSlot;

            _repository.Save(_examSlots);
            NotifyObservers();
            return oldExamSlot;
        }

        

    }
    
}
