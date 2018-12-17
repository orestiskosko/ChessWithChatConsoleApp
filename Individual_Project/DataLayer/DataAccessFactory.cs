using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataLayer
{
    public class DataAccessFactory
    {
        public IDataAccess GetDataAccess()
        {
            switch (ConfigurationManager.AppSettings.Get("DatabasePersistence"))
            {
                case "SqlExpressDb":
                    return new DbAccess();
                default:
                    return null;
            }
        }
    }
}
