using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Logging;
using DataLayer.Models;

namespace Individual_Project
{
    public class UserB_MenuModel : UserA_MenuModel
    {
        public List<Message> GetRecentMessages()
        {
            return _dataAccess.GetSentMessages(Global.CurrentUser.Username).Take(50).ToList();
        }

        public bool EditMessage(int messageId, string newMessageData)
        {
            return _dataAccess.EditMessage(messageId, newMessageData);
        }
    }
}
