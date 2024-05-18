using LangLang.Aplication.UseCases;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using System;

namespace LangLang.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public Profile Sender { get; set; }
        public Profile Recipient { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderInfo {get; set;}
        public MessageDTO() { }

        public Message ToMessage()
        {
            return new Message(Id, Sender, Recipient, Text);
        }

        public MessageDTO(Message message)
        {
            Id = message.Id;
            var profileService = new ProfileService();
            Sender = profileService.GetProfile(message.SenderId, message.SenderRole);
            Recipient = profileService.GetProfile(message.RecipientId, message.RecipientRole);
            Text = message.Text;
            SentAt = message.SentAt;
            SenderInfo = Sender.Name + " " + Sender.LastName + " " + Sender.Role.ToString();
        }
    }
}
