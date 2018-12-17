using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Individual_Project
{
    public class LoginModel
    {
        private IDataAccess _dataAccess = new DataAccessFactory().GetDataAccess();

        /// <summary>
        /// Validates an existing user.
        /// </summary>
        /// <param name="userToValidate"></param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password, out User userFound)
        {
            return _dataAccess.ValidateUser(username, password, out userFound);
        }
    }
}
