
using LangLang.Core.Repository.Serialization;
using LangLang.Domain.Models;
using System;

namespace LangLang.Core.Model
{
    public class Message : ISerializable
    {
        public int Id { get; set; }
        public int SenderId {  get; set; }
        public int RecipientId { get; set; }
        public UserType SenderRole { get; set; }
        public UserType RecipientRole {  get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public Message() { }
        public Message(int id, Profile sender, Profile recipient, string Text)
        {
            Id = id;
            SenderId = sender.Id;
            RecipientId = recipient.Id;
            SenderRole = sender.Role;
            RecipientRole = recipient.Role;
            this.Text = Text;
            SentAt = DateTime.Now;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            SenderId = int.Parse(values[1]);
            RecipientId = int.Parse(values[2]);
            SenderRole = (UserType)Enum.Parse(typeof(UserType), values[3]);
            RecipientRole = (UserType)Enum.Parse(typeof(UserType), values[4]);
            Text = values[5];

            try {
                SentAt = DateTime.ParseExact(values[6], Constants.DATE_TIME_FORMAT, null);
            } catch (FormatException) {
                throw new Exception("Date is not in correct format.");
            }

        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                SenderId.ToString(),
                RecipientId.ToString(),
                SenderRole.ToString(),
                RecipientRole.ToString(),
                Text,
                SentAt.ToString(Constants.DATE_TIME_FORMAT),
            };
        }
    }
}
