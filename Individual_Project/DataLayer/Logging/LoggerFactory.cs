using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataLayer.Logging
{
    public class LoggerFactory
    {

        /// <summary>
        /// Returns a message logger.
        /// </summary>
        /// <returns></returns>
        public ILogger GetMessageLogger()
        {
            string saveSource = ConfigurationManager.AppSettings.Get("MessageLogPath");
            return new FileLogger(saveSource);
        }


        /// <summary>
        /// Returns an error logger.
        /// </summary>
        /// <returns></returns>
        public ILogger GetErrorLogger()
        {
            string saveSource = ConfigurationManager.AppSettings.Get("ErrorLogPath");
            return new FileLogger(saveSource);
        }

    }
}
