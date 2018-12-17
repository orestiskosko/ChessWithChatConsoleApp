using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project
{
    public class MainAppModel
    {
        public LoginView LoginView = new LoginView();
        public UserA_MenuView UserA_MenuView = new UserA_MenuView();
        public UserB_MenuView UserB_MenuView = new UserB_MenuView();
        public UserC_MenuView UserC_MenuView = new UserC_MenuView();
        public SuperuserMenuView SuperuserMenuView = new SuperuserMenuView();
    }
}
