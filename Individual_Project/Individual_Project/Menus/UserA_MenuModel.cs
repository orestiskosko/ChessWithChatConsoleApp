using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using DataLayer;
using DataLayer.Logging;
using DataLayer.Models;

namespace Individual_Project
{
    public class UserA_MenuModel
    {

        protected IDataAccess _dataAccess = new DataAccessFactory().GetDataAccess();
        protected ILogger messageLogger = new LoggerFactory().GetMessageLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SendMessage(string receiverUsername, string message)
        {
            // Send message if validation passed.
            if (_dataAccess.FindUser(receiverUsername, out User receiver))
            {
                // Check if you are sending a message to yourself
                if (receiver.Username.Equals(Global.CurrentUsername))
                    return false;

                Message m = new Message()
                {
                    Tstamp = DateTime.Now,
                    Sender = Global.CurrentUser.Username,
                    Receiver = receiver.Username,
                    Data = message
                };

                // Write to database and message logger
                if ((_dataAccess.AddMessage(m)))
                {
                    messageLogger.Log($"{m.Tstamp}\t{m.Sender}\t{m.Receiver}\t{m.Data}");
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Message> ViewMessages()
        {
            return _dataAccess.GetInboxMessages(Global.CurrentUsername);
        }

        /// <summary>
        /// Finds a user with a specific username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User FindUser(string username)
        {
            _dataAccess.FindUser(username, out User userFound);
            return userFound;
        }


        /// <summary>
        /// Returns a string with all users.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _dataAccess.GetAppUsers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<Game> GetGames(string username)
        {
            return _dataAccess.GetGames(username);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="boardState"></param>
        /// <returns></returns>
        public bool SetGameStatus(int Id, string turn, string boardState, bool inProgress = true, string winner = null)
        {
            return _dataAccess.UpdateGame(Id, turn, boardState, inProgress, winner);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="opponentUsername"></param>
        /// <param name="boardData"></param>
        public bool CreateNewGame(string opponentUsername, string boardData, out Side assignedSide)
        {
            // Set a random out variable for now incase the opponent username is not validated
            assignedSide = Side.White;

            // Validate selected opponent username
            if (!_dataAccess.FindUser(opponentUsername, out User foundUser))
                return false;

            // Assign a random side to user who created game
            bool mySide = (new Random()).Next(0, 2) == 1 ? true : false;
            string whitePlayer = string.Empty;
            string blackPlayer = string.Empty;

            if (mySide)
            {
                whitePlayer = Global.CurrentUsername;
                assignedSide = Side.White;
                blackPlayer = opponentUsername;
            }
            else
            {
                blackPlayer = Global.CurrentUsername;
                assignedSide = Side.Black;
                whitePlayer = opponentUsername;
            }

            // Save game to database
            return _dataAccess.CreateGame(whitePlayer, blackPlayer, boardData);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteGame(int id)
        {
            return _dataAccess.DeleteGame(id);
        }
    }
}
