using LangLang.Core.Controller;
using LangLang.Core.Model;
using System;

namespace LangLang.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public Profile Sender {  get; set; }
        public Profile Recipient { get; set; }
        public string Text {  get; set; }
        public DateTime SentAt {  get; set; }
        public MessageDTO() { }

        public Message ToMessage()
        {
            return new Message(Id, Sender, Recipient, Text);
        }

        public MessageDTO(Message message, AppController appController)
        {
            Id = message.Id;
            Sender = appController.GetProfile(message.SenderId, message.SenderRole);
            Recipient = appController.GetProfile(message.RecipientId, message.RecipientRole);
            Text = message.Text;
            SentAt = message.SentAt;
        }
    }
}
