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
    public class UserC_MenuView : UserB_MenuView
    {

        private UserC_MenuModel _userC_MenuModel = new UserC_MenuModel();


        /// <summary>
        /// 
        /// </summary>
        public UserC_MenuView() : base()
        {
            _actions.Add("Delete message");
        }


        /// <summary>
        /// 
        /// </summary>
        public override void DisplayMenu()
        {
            while (true)
            {
                PrintLoginInfo();
                Console.WriteLine($"Main Menu");

                for (int i = 0; i < _actions.Count; i++)
                    Console.WriteLine($"{i+1}. {_actions[i]}");

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

                if (selection.Key == ConsoleKey.D5 || selection.Key == ConsoleKey.NumPad5)
                    DisplayDeleteMessage();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayDeleteMessage()
        {
            PrintLoginInfo();
            Console.WriteLine($"Recent Messages");
            List<Message> messages = _userC_MenuModel.GetRecentMessages();

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


            if (_userC_MenuModel.DeleteMessage(messages[(int)selection].Id))
            {
                PrintAffirmativeMessage("Message deleted successfully.\nPress any key to continue...");
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
