using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace LangLang.Repositories
{
    public class TutorRatingRepository : ITutorRatingRepository
    {
        private Dictionary<int, TutorRating> _tutorRatings;
        private const string _filePath = Constants.FILENAME_PREFIX + "tutorRatings.csv";
        public TutorRatingRepository()
        {
            _tutorRatings = Load();
        }

        public TutorRating Get(int id)
        {
            return _tutorRatings[id];
        }

        public List<TutorRating> GetAll()
        {
            return _tutorRatings.Values.ToList();
        }

        public void Add(TutorRating rating)
        {
            if (rating.Rating < Constants.MIN_GRADE || rating.Rating > Constants.MAX_GRADE)
            {
                throw new ArgumentException($"Rating must be between {Constants.MIN_GRADE} and {Constants.MAX_GRADE}.");
            }

            _tutorRatings.Add(rating.Id, rating);
            Save();
        }

        public Dictionary<int, TutorRating> Load()
        {
            Dictionary<int, TutorRating> tutorRatings = new();
            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(Constants.DELIMITER);

                    int id = int.Parse(parts[0]);
                    var rating = new TutorRating( id,
                                        int.Parse(parts[1]),
                                        int.Parse(parts[2]),
                                        int.Parse(parts[3]),
                                        int.Parse(parts[4]));

                        tutorRatings.Add(id, rating);
                    }
                }
            return tutorRatings;
        }

        public void Save()
        {
            using var writer = new StreamWriter(_filePath);

            foreach (var rating in GetAll())
            {

                var line = string.Join(Constants.DELIMITER.ToString(),
                                   rating.Id,
                                   rating.CourseId,
                                   rating.TutorId,
                                   rating.StudentId,
                                   rating.Rating);
                writer.WriteLine(line);
            }
        }
    }
}
