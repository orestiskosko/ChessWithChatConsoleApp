using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using DataLayer;
using DataLayer.Logging;
using DataLayer.Models;

namespace Individual_Project
{
    public class MainAppView
    {
        private static MainAppModel mainAppModel = new MainAppModel();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            while (true)
            {
                // Initialize view model.
                mainAppModel = new MainAppModel();
                Global.CurrentUser = null;

                // Start login.
                while (Global.CurrentUser == null)
                    Global.CurrentUser = mainAppModel.LoginView.DisplayLogin();

                // Display main menu for current user.
                switch ((AccessRights)Global.CurrentUser.Rights)
                {
                    case AccessRights.Superuser:
                        mainAppModel.SuperuserMenuView.DisplayMenu();
                        break;
                    case AccessRights.A:
                        mainAppModel.UserA_MenuView.DisplayMenu();
                        break;
                    case AccessRights.B:
                        mainAppModel.UserB_MenuView.DisplayMenu();
                        break;
                    case AccessRights.C:
                        mainAppModel.UserC_MenuView.DisplayMenu();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}