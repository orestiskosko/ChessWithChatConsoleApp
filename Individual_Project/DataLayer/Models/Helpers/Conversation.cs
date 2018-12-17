using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Conversation
    {
        private IDataAccess dataAccess = new DataAccessFactory().GetDataAccess();

        public List<Message> GetMessages(string senderUsername, string receiverUsername)
        {
            List<Message> conv = new List<Message>();

            List<Message> msgOfSender = dataAccess
                .GetInboxMessages(senderUsername)
                .Where(m => m.Sender.Equals(receiverUsername))
                .ToList();

            List<Message> msgOfReceiver = dataAccess
                .GetInboxMessages(receiverUsername)
                .Where(m => m.Sender.Equals(senderUsername))
                .ToList();

            msgOfSender.ForEach(x => conv.Add(x));
            msgOfReceiver.ForEach(x => conv.Add(x));
            return conv.OrderBy(m => m.Tstamp).ToList();

        }
    }
}
