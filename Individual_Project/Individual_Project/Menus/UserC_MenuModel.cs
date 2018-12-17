using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project
{
    public class UserC_MenuModel : UserB_MenuModel
    {
        public bool DeleteMessage(int messageId)
        {
            return _dataAccess.DeleteMessage(messageId);
        }
    }
}
