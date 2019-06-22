using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace VolunteersBase.UsersClasses
{
    class UserCoordinator
    {
        private static string datasource = @"DESKTOP-5IPEUIL\SQLEXPRESS";
        private static string database = "VolunteersBase";
        private static string username = "coor";
        private static string password = "123";

        public static SqlConnection GetDBConnection()
        {
            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }
    }
}
