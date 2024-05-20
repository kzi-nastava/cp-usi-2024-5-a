using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using LangLang.Core;
using System.Linq;
using System.IO;
using LangLang.Core.Model;
using System;

namespace LangLang.Repositories
{
    public class TutorRepository : Subject, ITutorRepository
    {

        private readonly Dictionary<int, Tutor> _tutors;
        private string _filePath = Constants.FILENAME_PREFIX + "tutors.csv";

        public TutorRepository()
        {
            _tutors = Load();
        }

        public void Add(Tutor tutor)
        {
            _tutors.Add(tutor.Profile.Id, tutor);
            Save();
            NotifyObservers();
        }

        public void Deactivate(int id)
        {
            Tutor tutor = Get(id);
            if (tutor == null) return;

            _tutors[id].Profile.IsActive = false;

            Save();
            NotifyObservers();
        }

        public Tutor Get(int id)
        {
            return _tutors[id];
        }

        public List<Tutor> GetAll()
        {
            return _tutors.Values.ToList();
        }

        public void Save()
        {
            using var writer = new StreamWriter(_filePath);

            foreach (var tutor in GetAll())
            {
                var csvData = ToCSV(tutor);
                var line = string.Join(Constants.DELIMITER.ToString(), csvData);
                writer.WriteLine(line);
            }
        }

        public void Update(Tutor tutor)
        {
            Tutor oldTutor = Get(tutor.Profile.Id);
            if (oldTutor == null) return;

            oldTutor.Profile.Name = tutor.Profile.Name;
            oldTutor.Profile.LastName = tutor.Profile.LastName;
            oldTutor.Profile.Gender = tutor.Profile.Gender;
            oldTutor.Profile.BirthDate = tutor.Profile.BirthDate;
            oldTutor.Profile.PhoneNumber = tutor.Profile.PhoneNumber;
            oldTutor.Profile.Email = tutor.Profile.Email;
            oldTutor.Profile.Password = tutor.Profile.Password;
            oldTutor.Profile.Role = tutor.Profile.Role;
            oldTutor.EmploymentDate = tutor.EmploymentDate;

            Save();
            NotifyObservers();
        }

        // NOTE: The methods below are temporary until connecting to the database.

        public string[] ToCSV(Tutor tutor)
        {
            List<string> csvValues = new List<string>();

            csvValues.Add(tutor.Profile.ToString());
            csvValues.Add(tutor.EmploymentDate.ToString("yyyy-MM-dd"));
            if (tutor.Skill.Language.Count > 0)
            {
                csvValues.Add(ListToCSV(tutor.Skill));
            }

            return csvValues.ToArray();
        }

        public string ListToCSV(Skill skill)
        {
            List<string> languageLevels = new List<string>();

            for (int i = 0; i < skill.Language.Count; ++i)
            {
                string languageLevel = $"{skill.Language[i]},{skill.Level[i]}";
                languageLevels.Add(languageLevel);
            }

            return string.Join("|", languageLevels);
        }

        public Dictionary<int, Tutor> Load()
        {
            var tutors = new Dictionary<int, Tutor>();

            using (var reader = new StreamReader(_filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(Constants.DELIMITER);

                    if (values.Length < 11)
                    {
                        continue;
                    }

                    var profile = new Profile(
                        values[0], values[1], values[2], values[3], values[4],
                        values[5], values[6], values[7], values[8], values[9]
                    );

                    DateTime employmentDate;
                    try
                    {
                        employmentDate = DateTime.ParseExact(values[10], Constants.DATE_FORMAT, null);
                    }
                    catch
                    {
                        throw new FormatException("Date is not in the correct format.");
                    }

                    var skill = new Skill();

                    for (int i = 11; i < values.Length; i++)
                    {
                        string[] languageSkill = values[i].Split(',');

                        if (languageSkill.Length != 2)
                        {
                            throw new FormatException("Language and skill pair is not in the correct format.");
                        }

                        try
                        {
                            LanguageLevel level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), languageSkill[1]);
                            skill.Language.Add(languageSkill[0]);
                            skill.Level.Add(level);
                        }
                        catch (ArgumentException)
                        {
                            throw new FormatException("Invalid skill level.");
                        }
                    }

                    var tutor = new Tutor
                    {
                        Profile = profile,
                        EmploymentDate = employmentDate,
                        Skill = skill
                    };

                    tutors.Add(tutors.Count, tutor);
                }
            }

            return tutors;
        }
    }
}
