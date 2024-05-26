using LangLang.Configuration;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.IO;

namespace LangLang.Repositories
{
    public class EmailRepository : IEmailRepository
    {

        private Dictionary<string, string> _emails;
        private string _filePath = Constants.FILENAME_PREFIX + "/EmailTemplates";

        public EmailRepository() {
            Load();
        }

        public void Load()
        {
            _emails = new Dictionary<string, string>();

            var subject = File.ReadAllText(_filePath + "/subject.csv");
            _emails["subject"] = subject;

            var failingMessage = File.ReadAllText(_filePath + "/failingMessage.csv");
            _emails["failingMessage"] = failingMessage;

            var passingMessage = File.ReadAllText(_filePath + "/passingMessage.csv");
            _emails["passingMessage"] = passingMessage;
        }

        public string GetContent(string type)
        {
            return _emails[type];
        }

    }
}
