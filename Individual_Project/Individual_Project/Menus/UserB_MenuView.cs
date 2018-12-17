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
    public class UserB_MenuView : UserA_MenuView
    {
        private UserB_MenuModel _userB_MenuModel = new UserB_MenuModel();


        /// <summary>
        /// 
        /// </summary>
        public UserB_MenuView() : base()
        {
            _actions.Add("Edit message");
        }


        /// <summary>
        /// 
        /// </summary>
        public override void DisplayMenu()
        {
            while (true)
            {
                PrintLoginInfo();

                Console.WriteLine("Main Menu");
                for (int i = 0; i < _actions.Count; i++)
                    Console.WriteLine($"{i + 1}. {_actions[i]}");

                Console.Write("Select action (ESC to exit): ");

                ConsoleKeyInfo selection = Console.ReadKey();

                if (selection.Key == ConsoleKey.Escape)
                    return;

                if (selection.Key == ConsoleKey.D1 || selection.Key == ConsoleKey.NumPad1)
                    DisplayGameSetup();

                if (selection.Key == ConsoleKey.D2 || selection.Key == ConsoleKey.NumPad2)
                    DisplaySendMessage();

                if (selection.Key == ConsoleKey.D3 || selection.Key == ConsoleKey.NumPad3)
                    DisplayViewMessage();

                if (selection.Key == ConsoleKey.D4 || selection.Key == ConsoleKey.NumPad4)
                    DisplayEditMessage();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayEditMessage()
        {
            PrintLoginInfo();

            Console.WriteLine($"Recent Messages");
            List<Message> messages = _userB_MenuModel.GetRecentMessages();

            for (int i = 0; i < messages.Count; i++)
            {
                Console.WriteLine($"{i} - {messages[i].Tstamp} - To: {messages[i].Receiver} - Message: {messages[i].Data}");
            }

            Console.Write("\n\nSelect message (number) you want to edit: ");
            uint selection;

            if (!uint.TryParse(Console.ReadLine(), out selection) || (selection > messages.Count - 1))
            {
                PrintNegativeMessage("Invalid input.\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter new message: ");
            if (_userB_MenuModel.EditMessage(messages[(int)selection].Id, Console.ReadLine()))
            {
                PrintAffirmativeMessage("Message edited successfully.\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                PrintNegativeMessage("Invalid input.\nPress any key to continue...");
                Console.ReadKey();
            }

        }
    }
}
