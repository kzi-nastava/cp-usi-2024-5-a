
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class MessageController
    {
        private readonly MessageDAO _messages;

        public MessageController()
        {
            _messages = new MessageDAO();
        }

        public List<Message> GetAll()
        {
            return _messages.GetAll();
        }

        public Message? Get(int id)
        {
            return _messages.Get(id);
        }

        public void Add(Message message)
        {
            _messages.AddMessage(message);
        }

        public List<Message> GetReceivedMessages(Student student)
        {
            return _messages.GetReceivedMessages(student);
        }

        public List<Message> GetReceivedMessages(Tutor tutor)
        {
            return _messages.GetReceivedMessages(tutor);
        }

        public List<Message> GetReceivedMessages(Director director)
        {
            return _messages.GetReceivedMessages(director);
        }
    }
}
