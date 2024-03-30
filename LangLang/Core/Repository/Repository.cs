using LangLang.Core.Repository.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Repository
{
    public class Repository<T> where T : ISerializable, new()
    {
        private readonly string _fileName = @"../../../../LangLang/Data/{0}";
        private readonly Serializer<T> _serializer = new Serializer<T>();

        public Repository(string fileName)
        {
            _fileName = string.Format(_fileName, fileName);
        }
        public Dictionary<int,T> Load()
        {
            if (!File.Exists(_fileName))
            {
                FileStream fs = File.Create(_fileName);
                fs.Close();
            }

            IEnumerable<string> lines = File.ReadLines(_fileName);
            Dictionary<int,T> objects = _serializer.FromCSV(lines);

            return objects;
        }

        public void Save(Dictionary<int,T> objects)
        {
            string serializedObjects = _serializer.ToCSV(objects);
            using (StreamWriter streamWriter = new StreamWriter(_fileName))
            {
                streamWriter.Write(serializedObjects);
            }
        }
    }
}
