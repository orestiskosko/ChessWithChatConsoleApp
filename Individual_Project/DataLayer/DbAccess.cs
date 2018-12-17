using DataLayer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace DataLayer
{
    public class DbAccess : IDataAccess
    {
        private ILogger errorLogger = (new LoggerFactory()).GetErrorLogger();
        private ILogger messageLogger = (new LoggerFactory()).GetMessageLogger();

        #region "Users"
        public bool CreateUser(User newUser)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public bool DeleteUser(string username)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    // Find user
                    User u = db.Users.Find(username);

                    // Delete all his messages
                    List<Message> msg = db.Messages.Where(m => m.Sender.Equals(username) || m.Receiver.Equals(username)).ToList();
                    msg.ForEach(m => db.Messages.Remove(m));

                    // Delete all his games
                    List<Game> gs = db.Games.Where(g => g.WhitePlayer.Equals(username) || g.BlackPlayer.Equals(username)).ToList();
                    gs.ForEach(g => db.Games.Remove(g));

                    // Delete user
                    db.Users.Remove(u);
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public bool FindUser(string username, out User user)
        {
            user = null;
            try
            {
                using (var db = new AppDbContext())
                    user = db.Users.Find(username);
                return user != null;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public List<User> GetAppUsers()
        {
            try
            {
                List<User> u;
                using (var db = new AppDbContext())
                    u = db.Users.ToList();
                return u;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return null;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return null;
            }
        }


        public bool UpdateUser(string username, User updateInfo)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    User temp = db.Users.Find(username);
                    temp.Username = updateInfo.Username;
                    temp.Password = updateInfo.Password;
                    temp.FirstName = updateInfo.FirstName;
                    temp.LastName = updateInfo.LastName;
                    temp.Email = updateInfo.Email;
                    temp.Rights = updateInfo.Rights;
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public bool ValidateUser(string username, string password, out User user)
        {
            user = null;
            try
            {
                using (var db = new AppDbContext())
                    user = db.Users.Where(u => u.Username.Equals(username) && u.Password.Equals(password)).FirstOrDefault();
                return user != null;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }
        #endregion

        #region "Messages"
        public bool AddMessage(Message message)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Messages.Add(message);
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public bool DeleteMessage(int messageId)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    Message message = db.Messages.Find(messageId);
                    db.Messages.Remove(message);
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
        }


        public bool EditMessage(int messageId, string newMessageData)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    Message message = db.Messages.Find(messageId);
                    message.Data = newMessageData;
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public List<Message> GetInboxMessages(string username)
        {
            try
            {
                List<Message> msg;
                using (var db = new AppDbContext())
                    msg = db.Messages.Where(m => m.Receiver.Equals(username)).ToList();
                return msg;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return null;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return null;
            }
        }


        public List<Message> GetSentMessages(string username)
        {
            try
            {
                List<Message> msg;
                using (var db = new AppDbContext())
                    msg = db.Messages.Where(m => m.Sender.Equals(username)).ToList();
                return msg;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return null;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return null;
            }
        }
        #endregion

        #region "Games"
        public bool CreateGame(string whitePlayer, string blackPlayer, string boardState)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Games.Add(new Game()
                    {
                        InProgress = true,
                        WhitePlayer = whitePlayer,
                        BlackPlayer = blackPlayer,
                        Turn = whitePlayer,
                        BoardData = boardState
                    });
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public bool DeleteGame(int id)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    Game g = db.Games.Find(id);
                    db.Games.Remove(g);
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public Game FindGame(int id)
        {
            try
            {
                Game g;
                using (var db = new AppDbContext())
                    g = db.Games.Find(id);
                return g;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return null;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return null;
            }
        }


        public List<Game> GetGames(string username)
        {
            try
            {
                List<Game> gs;
                using (var db = new AppDbContext())
                {
                    gs = db.Games.Where(x => (x.WhitePlayer.Equals(username) || x.BlackPlayer.Equals(username)) && x.InProgress)
                                .Distinct()
                                .ToList();
                }
                return gs;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return null;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return null;
            }
        }


        public bool UpdateGame(int id, string turn, string boardState, bool inProgress, string winner)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    Game g = db.Games.Find(id);
                    g.InProgress = inProgress;
                    g.BoardData = boardState;
                    g.Turn = turn;
                    g.Winner = winner;
                    db.SaveChanges();
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return false;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return false;
            }
        }


        public string GetGameState(int Id)
        {
            try
            {
                string g;
                using (var db = new AppDbContext())
                    g = db.Games.Find(Id).BoardData;
                return g;
            }
            catch (DbEntityValidationException e)
            {
                errorLogger.Log(e.Message);
                return null;
            }
            catch (DbUpdateException e)
            {
                errorLogger.Log(e.InnerException.InnerException.Message);
                return null;
            }
        }
        #endregion
    }
}
