
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IPenaltyPointRepository
    {
        public List<PenaltyPoint> GetAll();
        public PenaltyPoint Get(int id);
        public List<PenaltyPoint> GetPenaltyPoints(Student student);
        public PenaltyPoint GetOldestPenaltyPoint(List<PenaltyPoint> points);

        public void Add(PenaltyPoint point);
        public void Delete(PenaltyPoint point);




    }
}
