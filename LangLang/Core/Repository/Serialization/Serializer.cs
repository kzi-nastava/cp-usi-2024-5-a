using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Repository.Serialization
{
    class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        public string ToCSV(Dictionary<int,T> objects)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ISerializable obj in objects.Values)
            {
                string line = string.Join(Delimiter.ToString(), obj.ToCSV());
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        public Dictionary<int,T> FromCSV(IEnumerable<string> lines)
        {
            Dictionary<int,T> objects = new Dictionary<int, T>();

            foreach (string line in lines)
            {
                string[] csvValues = line.Split(Delimiter);
                T obj = new T();
                obj.FromCSV(csvValues);
                objects.Add(obj.Id,obj);
            }

            return objects;
        }
    }
}
