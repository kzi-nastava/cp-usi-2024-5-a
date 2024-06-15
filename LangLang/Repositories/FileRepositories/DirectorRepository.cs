using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using LangLang.Configuration;

namespace LangLang.Repositories.FileRepositories
{
    public class DirectorRepository : IDirectorRepository
    {

        private readonly Dictionary<int, Director> _directors;
        private string _filePath = Constants.FILENAME_PREFIX + "directors.csv";
        public DirectorRepository()
        {
            _directors = Load();
        }
        public List<Director> GetAll()
        {
            return _directors.Values.ToList();
        }

        public Director Get(int id)
        {
            return _directors[id];
        }

        public void Save()
        {
            using var writer = new StreamWriter(_filePath);

            foreach (var director in GetAll())
            {
                var line = director.Profile.ToString();
                writer.WriteLine(line);
            }
        }

        public Dictionary<int, Director> Load()
        {
            var directors = new Dictionary<int, Director>();

            if (!File.Exists(_filePath)) return directors;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var tokens = line.Split(Constants.DELIMITER);

                var profile = new Profile(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4],
                                          tokens[5], tokens[6], tokens[7], tokens[8], tokens[9]);

                var director = new Director();
                director.Profile = profile;

                directors.Add(director.Id, director);
            }

            return directors;
        }

    }
}
