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

            var subject = File.ReadAllText(_filePath + "/resultSubject.csv");
            _emails["resultSubject"] = subject;

            var failingMessage = File.ReadAllText(_filePath + "/failingMessage.csv");
            _emails["failingMessage"] = failingMessage;

            var passingMessage = File.ReadAllText(_filePath + "/passingMessage.csv");
            _emails["passingMessage"] = passingMessage;

            var gratitudeMessage = File.ReadAllText(_filePath + "/gratitudeMessage.csv");
            _emails["gratitudeMessage"] = gratitudeMessage;

            var gratitudeSubject = File.ReadAllText(_filePath + "/gratitudeSubject.csv");
            _emails["gratitudeSubject"] = gratitudeSubject;

        }

        public string GetContent(string type)
        {
            return _emails[type];
        }

    }
}
