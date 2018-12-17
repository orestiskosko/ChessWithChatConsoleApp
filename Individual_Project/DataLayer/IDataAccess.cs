using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;

namespace DataLayer
{
    public interface IDataAccess
    {
        // Users
        List<User> GetAppUsers();
        bool CreateUser(User newUser);
        bool DeleteUser(string username);
        bool FindUser(string username, out User user);
        bool ValidateUser(string username, string password, out User user);
        bool UpdateUser(string username, User updateInfo);

        // Messages
        bool AddMessage(Message message);
        List<Message> GetInboxMessages(string username);
        List<Message> GetSentMessages(string username);
        bool EditMessage(int messageId, string newMessageData);
        bool DeleteMessage(int messageId);

        // Games
        bool CreateGame(string whitePlayer, string blackPlayer, string boardState);
        bool DeleteGame(int id);
        Game FindGame(int id);
        List<Game> GetGames(string username);
        bool UpdateGame(int id, string turn, string boardState, bool inProgress, string winner);
        string GetGameState(int Id);
    }
}
