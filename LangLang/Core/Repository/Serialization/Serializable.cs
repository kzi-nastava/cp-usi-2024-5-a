using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Repository.Serialization
{
    public interface ISerializable
    {
        void FromCSV(string[] values);
        string[] ToCSV();
    }
}
