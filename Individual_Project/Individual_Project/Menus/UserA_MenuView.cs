using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Logging;
using DataLayer.Models;
using Chess;

namespace Individual_Project
{
    public class UserA_MenuView
    {
        private UserA_MenuModel _userA_MenuModel = new UserA_MenuModel();
        protected List<string> _actions = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public UserA_MenuView()
        {
            _actions.Add("Play chess");
            _actions.Add("Send message");
            _actions.Add("View message");
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void DisplayMenu()
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
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayGameSetup()
        {
            while (true)
            {
                PrintLoginInfo();

                Console.WriteLine("1. Start new game");
                Console.WriteLine("2. Resume game");
                Console.WriteLine("3. Delete game");
                Console.Write("\nChoose action (ESC to exit): ");

                ConsoleKeyInfo selection = Console.ReadKey();

                if (selection.Key == ConsoleKey.Escape)
                    break;

                if (selection.Key == ConsoleKey.D1 || selection.Key == ConsoleKey.NumPad1)
                    DisplayStartNewGame();

                if (selection.Key == ConsoleKey.D2 || selection.Key == ConsoleKey.NumPad2)
                    DisplayResumeGame(false);

                if (selection.Key == ConsoleKey.D3 || selection.Key == ConsoleKey.NumPad3)
                    DisplayDeleteGame();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayStartNewGame()
        {
            PrintAllUsers();

            Console.Write("\nChoose your opponent's username: ");
            string opponentUsername = Console.ReadLine();

            // Logic to avoid starting a game with yourself
            if (opponentUsername.Equals(Global.CurrentUsername))
            {
                PrintNegativeMessage("You can't play against with yourself!\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            ChessGame newGame = new ChessGame();
            if (_userA_MenuModel.CreateNewGame(opponentUsername, newGame.GetBoardState(), out Side assignedSide))
            {
                PrintAffirmativeMessage($"New chess game created successfully! You were assigned {assignedSide.ToString()}.\nPress any key to continue...");
                Console.ReadKey();
                DisplayResumeGame(true);
            }
            else
            {
                PrintNegativeMessage("Game creation was unsuccessfull!\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayResumeGame(bool isNewlyCreated)
        {
            PrintLoginInfo();

            // Intialize necessary variables
            Game storedGame = null;
            List<Game> myGames = _userA_MenuModel.GetGames(Global.CurrentUser.Username);
            int storedGameId = 0;

            if (!isNewlyCreated)
            {
                Console.WriteLine("Active games:");

                foreach (Game g in myGames)
                {
                    Console.Write($"{g.Id} - ");

                    if (g.WhitePlayer.Equals(Global.CurrentUser.Username))
                        Console.Write($"Vs {g.BlackPlayer}(RED)");
                    else
                        Console.Write($"Vs {g.WhitePlayer}(GREEN)");

                    if (g.Turn.Equals(Global.CurrentUser.Username))
                        Console.WriteLine($" It's your turn.");
                    else
                        Console.WriteLine($" Opponent is playing.");
                }

                Console.Write("\nSelect a match ID: ");
                int selectedMatchId = 0;
                if (!int.TryParse(Console.ReadLine(), out selectedMatchId))
                {
                    PrintNegativeMessage("Invalid input.\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                // Get stored chessgame
                storedGame = myGames.Where(x => x.Id.Equals(selectedMatchId)).FirstOrDefault();
            }
            else // newly created game
            {
                // Find most recently created game
                storedGame = myGames.OrderByDescending(x => x.Id).FirstOrDefault();
            }

            // Validate selected game Id
            if (storedGame == null)
            {
                PrintNegativeMessage($"Invalid selection!\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // Save current game's Id
            storedGameId = storedGame.Id;

            //
            // Implement pseudo-realtime feature
            //
            ChessGame game = new ChessGame();
            while (!Console.KeyAvailable)
            {
                // Retrieve game info from db
                storedGame = _userA_MenuModel.GetGames(Global.CurrentUsername).Where(g => g.Id.Equals(storedGameId)).FirstOrDefault();


                // Determine current user's side
                Side mySide = Side.Black;
                if (storedGame.WhitePlayer.Equals(Global.CurrentUsername))
                    mySide = Side.White;

                // Initialize and restore game
                game.CurrentUser = Global.CurrentUsername;
                game.CurrentSide = mySide;
                game.TurnUser = storedGame.Turn;
                game.SetBoardState(storedGame.BoardData);


                // Decide if a valid move was done
                while (!game.Play()) { }

                // Store current game's status only if is my turn
                if (game.TurnUser.Equals(Global.CurrentUsername))
                {
                    string turn;
                    if (storedGame.Turn.Equals(storedGame.WhitePlayer))
                        turn = storedGame.BlackPlayer;
                    else
                        turn = storedGame.WhitePlayer;

                    _userA_MenuModel.SetGameStatus(storedGame.Id, turn, game.GetBoardState());
                }

                // Check for winner
                if (game.Winner != null)
                    break;

                // wait
                System.Threading.Thread.Sleep(500);
            }

            // Finalize game if its over
            if (game.Winner != null)
            {
                // Retrieve game info from db
                storedGame = _userA_MenuModel.GetGames(Global.CurrentUsername).Where(g => g.Id.Equals(storedGameId)).FirstOrDefault();

                // Decide on winner's username
                string winner = game.Winner == Side.White ? storedGame.WhitePlayer : storedGame.BlackPlayer;

                // Finalize game
                _userA_MenuModel.SetGameStatus(storedGame.Id, null, game.GetBoardState(), false, winner);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayDeleteGame()
        {
            PrintLoginInfo();

            Console.WriteLine("Active games:");

            List<Game> myGames = _userA_MenuModel.GetGames(Global.CurrentUsername);
            foreach (Game g in myGames)
            {
                Console.Write($"{g.Id} - ");

                if (g.WhitePlayer.Equals(Global.CurrentUsername))
                    Console.Write($"Vs {g.BlackPlayer}(RED)");
                else
                    Console.Write($"Vs {g.WhitePlayer}(GREEN)");

                if (g.Turn.Equals(Global.CurrentUsername))
                    Console.WriteLine($" It's your turn.");
                else
                    Console.WriteLine($" Opponent is playing.");
            }

            Console.Write("\nSelect a match ID: ");
            int selectedMatchId = 0;
            if (!int.TryParse(Console.ReadLine(), out selectedMatchId))
            {
                PrintNegativeMessage("Invalid input.\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            if (_userA_MenuModel.DeleteGame(selectedMatchId))
            {
                PrintAffirmativeMessage("Game deleted successfully!\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                PrintNegativeMessage("Game was not deleted!\nPress any key to continue...");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplaySendMessage()
        {
            PrintLoginInfo();
            PrintAllUsers();

            Console.Write("To user: ");
            string receiverUsername = Console.ReadLine();

            Console.Write("Your message: ");
            string receiverMessage = Console.ReadLine();

            if (_userA_MenuModel.SendMessage(receiverUsername, receiverMessage))
                PrintAffirmativeMessage("Message sent!");
            else
                PrintNegativeMessage("Message not sent.");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        /// <summary>
        /// 
        /// </summary>
        public void DisplayViewMessage()
        {
            PrintLoginInfo();

            // Get user messages
            List<Message> messages = _userA_MenuModel.ViewMessages();

            // Return if no inbox messages.
            if (messages.Count == 0)
            {
                Console.WriteLine("No messages!\n\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // List senders if there are available messages.
            Console.WriteLine("Conversations");
            var groupedMessages = messages.GroupBy(m => m.Sender).ToList();
            for (int i = 0; i < groupedMessages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {groupedMessages[i].Key}");
            }

            // Select conversation
            Console.WriteLine("Select conversation (username): ");
            string senderUsername = Console.ReadLine();

            List<Message> conversation = new Conversation().GetMessages(Global.CurrentUser.Username, senderUsername);

            // Exit if conversation has zero messages
            if (conversation.Count == 0)
                return;

            Console.Clear();

            // Print conversation
            foreach (var m in conversation)
            {
                if (m.Sender.Equals(Global.CurrentUser.Username))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{m.Tstamp.ToString("dd/MM/yyyy HH:mm"),-70}");
                    Console.ResetColor();
                    Console.WriteLine($"{m.Sender + " \u25BA " + m.Data,-70}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{m.Tstamp.ToString("dd/MM/yyyy HH:mm"),70}");
                    Console.ResetColor();
                    Console.WriteLine($"{m.Sender + " \u25BA " + m.Data,70}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        #region "Generic Display Helpers"
        /// <summary>
        /// User logged in header for all screens.
        /// </summary>
        public void PrintLoginInfo()
        {
            Console.Clear();
            Console.WriteLine($"--- Logged in as: {Global.CurrentUsername} ---\n");
        }


        /// <summary>
        /// Prints a list of all users except current.
        /// </summary>
        public void PrintAllUsers()
        {
            PrintLoginInfo();
            List<User> users = _userA_MenuModel.GetAllUsers();
            int index = 1;
            foreach (User u in users)
            {
                if (!u.Username.Equals(Global.CurrentUsername))
                    Console.WriteLine($"{index}. {u.Username}"); index++;
            }
            Console.WriteLine();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void PrintAffirmativeMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void PrintNegativeMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion
    }
}
