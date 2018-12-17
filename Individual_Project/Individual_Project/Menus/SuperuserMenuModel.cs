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
    public class SuperuserMenuModel : UserC_MenuModel
    {
        /// <summary>
        /// Helper for getting all app users.
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return _dataAccess.GetAppUsers();
        }


        /// <summary>
        /// Helper for creating a new user.
        /// </summary>
        public bool CreateUser(User newUser)
        {
            return !string.IsNullOrWhiteSpace(newUser.Username) && _dataAccess.CreateUser(newUser);
        }


        /// <summary>
        /// Helper for viewing a user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="foundUser"></param>
        /// <returns></returns>
        public bool ViewUser(string username, out User foundUser)
        {
            return _dataAccess.FindUser(username, out foundUser);
        }


        /// <summary>
        /// Helper for deleting a user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool DeleteUser(string username)
        {


            if (!_dataAccess.FindUser(username, out User user))
                return false;

            return _dataAccess.DeleteUser(username);
        }


        /// <summary>
        /// Helper for changing a user's rights.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newRights"></param>
        /// <returns></returns>
        public bool ChangeUserRights(string username, AccessRights newRights)
        {
            if (!_dataAccess.FindUser(username, out User foundUser))
                return false;

            return _dataAccess.UpdateUser(username, new User()
            {
                FirstName = foundUser.FirstName,
                LastName = foundUser.LastName,
                Username = foundUser.Username,
                Password = foundUser.Password,
                Rights = (byte)newRights
            });
        }


        /// <summary>
        /// Helper for updating a user.
        /// </summary>
        /// <param name="usernameToUpdate"></param>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        public bool UpdateUser(string usernameToUpdate, User updateInfo)
        {
            if (!_dataAccess.FindUser(usernameToUpdate, out User user))
                return false;

            return _dataAccess.UpdateUser(usernameToUpdate, updateInfo);
        }
    }
}
