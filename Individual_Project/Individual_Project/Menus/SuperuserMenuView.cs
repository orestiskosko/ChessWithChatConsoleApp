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
    public class SuperuserMenuView : UserC_MenuView
    {
        // Model
        private SuperuserMenuModel _superuserMenuModel = new SuperuserMenuModel();


        /// <summary>
        /// Intializes the SuperuserMenuView class.
        /// </summary>
        public SuperuserMenuView() : base()
        {
            _actions.Add("Create user");
            _actions.Add("View user");
            _actions.Add("Delete user");
            _actions.Add("Update user info");
            _actions.Add("Change user rights");
        }


        /// <summary>
        /// Displays the superuser menu.
        /// </summary>
        public override void DisplayMenu()
        {
            while (true)
            {
                PrintLoginInfo();

                Console.WriteLine("Main Menu");
                for (int i = 0; i < _actions.Count; i++)
                    Console.WriteLine($"{i}. {_actions[i]}");

                Console.Write("Select action (ESC to exit): ");

                ConsoleKeyInfo selection = Console.ReadKey();

                if (selection.Key == ConsoleKey.Escape)
                    return;

                if (selection.Key == ConsoleKey.D0 || selection.Key == ConsoleKey.NumPad0)
                    DisplayGameSetup();

                if (selection.Key == ConsoleKey.D1 || selection.Key == ConsoleKey.NumPad1)
                    DisplaySendMessage();

                if (selection.Key == ConsoleKey.D2 || selection.Key == ConsoleKey.NumPad2)
                    DisplayViewMessage();

                if (selection.Key == ConsoleKey.D3 || selection.Key == ConsoleKey.NumPad3)
                    DisplayEditMessage();

                if (selection.Key == ConsoleKey.D4 || selection.Key == ConsoleKey.NumPad4)
                    DisplayDeleteMessage();

                if (selection.Key == ConsoleKey.D5 || selection.Key == ConsoleKey.NumPad5)
                    DisplayCreateUser();

                if (selection.Key == ConsoleKey.D6 || selection.Key == ConsoleKey.NumPad6)
                    DisplayViewUser();

                if (selection.Key == ConsoleKey.D7 || selection.Key == ConsoleKey.NumPad7)
                    DisplayDeleteUser();

                if (selection.Key == ConsoleKey.D8 || selection.Key == ConsoleKey.NumPad8)
                    DisplayUpdateUser();

                if (selection.Key == ConsoleKey.D9 || selection.Key == ConsoleKey.NumPad9)
                    DisplayChangeUserRights();
            }
        }


        /// <summary>
        /// Displays the change user rights screen.
        /// </summary>
        public void DisplayChangeUserRights()
        {
            PrintLoginInfo();
            PrintAllUsers();

            Console.Write("\nSelect username: ");
            string requestedUsername = Console.ReadLine();

            User user = _superuserMenuModel.FindUser(requestedUsername);
            if (user == null)
            {
                PrintNegativeMessage("Invalid selection.\nPress any key to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Current access rights: {(AccessRights)user.Rights}");

            Console.WriteLine("\nNew access rights: ");
            int i = 0;
            foreach (var item in Enum.GetValues(typeof(AccessRights)))
            {
                Console.WriteLine($"{i}. {item}");
                i++;
            }

            AccessRights newRights;
            while (!Enum.TryParse(Console.ReadLine(), out newRights))
            {
                PrintNegativeMessage("Invalid selection.\n");
                PrintAffirmativeMessage("Choose new access rights: ");
            }

            _superuserMenuModel.ChangeUserRights(requestedUsername, newRights);
            PrintAffirmativeMessage($"User {requestedUsername} access rights were updated successfully to {newRights}!\nPress any key to continue...");
            Console.ReadLine();
        }


        /// <summary>
        /// Displays the new user registration screen.
        /// </summary>
        public void DisplayCreateUser()
        {
            PrintLoginInfo();
            PrintLoginInfo();

            User newUser = new User();
            Console.WriteLine("New User Registration\n");
            Console.Write("Enter first name: ");
            newUser.FirstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            newUser.LastName = Console.ReadLine();
            Console.Write("Enter a username: ");
            newUser.Username = Console.ReadLine();
            Console.Write("Enter a password: ");
            newUser.Password = Console.ReadLine();

            // TODO Add an email field for new user. Change also for DbAccess

            if (_superuserMenuModel.CreateUser(newUser))
                PrintAffirmativeMessage("New user created successfully!\nPress any key to continue...");
            else
                PrintNegativeMessage("User was no created.\nPress any key to continue...");

            Console.ReadLine();
        }


        /// <summary>
        /// Displays the view user screen.
        /// </summary>
        private void DisplayViewUser()
        {
            PrintLoginInfo();
            PrintAllUsers();

            Console.Write("Select user: ");
            string requestedUsername = Console.ReadLine();
            _superuserMenuModel.ViewUser(requestedUsername, out User foundUser);

            if (foundUser != null)
            {
                Console.WriteLine("\n" + foundUser.DisplayInfo());
                Console.WriteLine("\n\nPress any key to continue...");
            }
            else
                PrintNegativeMessage("\nUser not found!\nPress any key to continue...");
            Console.ReadLine();
        }


        /// <summary>
        /// Displays the delete user screen.
        /// </summary>
        private void DisplayDeleteUser()
        {
            PrintLoginInfo();
            PrintAllUsers();

            Console.Write("Select user: ");
            string requestedUsername = Console.ReadLine();

            if (requestedUsername.Equals(Global.CurrentUsername))
                PrintNegativeMessage("Could not delete the requested user.\nPress any key to continue...");
            else if (_superuserMenuModel.DeleteUser(requestedUsername))
                PrintAffirmativeMessage($"User {requestedUsername} was succesfully deleted.\nPress any key to continue...");
            else
                PrintNegativeMessage("Could not delete the requested user.\nPress any key to continue...");

            Console.ReadLine();
        }


        /// <summary>
        /// Displays the update user screen.
        /// </summary>
        private void DisplayUpdateUser()
        {
            PrintLoginInfo();
            PrintAllUsers();

            Console.Write("Select user: ");
            string requestedUsername = Console.ReadLine();

            User existingUser = _superuserMenuModel.FindUser(requestedUsername);

            if (existingUser == null)
            {
                PrintNegativeMessage("Invalid input.\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            User updateInfo = new User();
            Console.Write($"New first name ({existingUser.FirstName}): ");
            updateInfo.FirstName = Console.ReadLine();
            Console.Write($"New last name ({existingUser.LastName}): ");
            updateInfo.LastName = Console.ReadLine();
            Console.Write($"New e-mail ({existingUser.Email}): ");
            updateInfo.Email = Console.ReadLine();

            // username is primary key
            updateInfo.Username = existingUser.Username;

            // Password. if empty same as before
            Console.Write($"New Password ({existingUser.Password}): ");
            updateInfo.Password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(updateInfo.Password))
                updateInfo.Password = existingUser.Password;

            // do update
            if (_superuserMenuModel.UpdateUser(requestedUsername, updateInfo))
                PrintAffirmativeMessage("User updated successfully!\nPress any key to continue...");
            else
                PrintNegativeMessage("Update failed.\nPress any key to continue...");
            Console.ReadKey();
        }

    }
}
