using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Individual_Project
{
    /// <summary>
    /// Only really important stuff goes here that need to be available everywhere.
    /// </summary>
    internal static class Global
    {
        public static User CurrentUser { get; set; }
        public static string CurrentUsername
        {
            get { return CurrentUser.Username; }
        }
    }
}
