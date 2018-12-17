using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Individual_Project
{
    public class LoginView
    {
        private LoginModel _loginModel = new LoginModel();


        /// <summary>
        /// Displays the login screen.
        /// </summary>
        public User DisplayLogin()
        {
            Console.Clear();
            Console.WriteLine("--- Login ---");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");

            string password = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    Console.Write("*");
                    password += key.KeyChar;
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            if (_loginModel.ValidateUser(username, password, out User userFound))
                return userFound;

            Console.Write("\nLogin failed. Press any key to try again...");
            Console.ReadLine();
            return null;
        }
    }
}
