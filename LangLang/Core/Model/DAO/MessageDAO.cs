using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class MessageDAO : Subject
    {
        private readonly Dictionary<int, Message> _messages;
        private readonly Repository<Message> _repository;
        
        public MessageDAO()
        {
            _repository = new Repository<Message>("messages.csv");
            _messages = _repository.Load();
        }

        private int GenerateId()
        {
            if (_messages.Count == 0) return 0;
            return _messages.Keys.Max() + 1;
        }

        public Message? Get(int id)
        {
            return _messages[id];
        }

        public List<Message> GetAll()
        {
            return _messages.Values.ToList();
        }

        public Message AddMessage(Message message)
        {
            message.Id = GenerateId();
            _messages.Add(message.Id, message);
            _repository.Save(_messages);
            NotifyObservers();
            return message;
        }

        public List<Message> GetReceivedMessages(Student student)
        {
            return GetReceivedMessages(student.Profile);
        }

        public List<Message> GetReceivedMessages(Tutor tutor)
        {
            return GetReceivedMessages(tutor.Profile);
        }

        public List<Message> GetReceivedMessages(Director director)
        {
            return GetReceivedMessages(director.Profile);
        }
    
        private List<Message> GetReceivedMessages(Profile profile)
        {
            return GetAll().Where(message => message.RecipientId == profile.Id 
                                && message.RecipientRole == profile.Role).ToList();
            
        }
    }
}
