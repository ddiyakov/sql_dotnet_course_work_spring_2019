using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace VolunteersBase.UsersClasses
{
    class DBSQLServerUtils
    {
        public static SqlConnection GetDBConnection(string datasource, string database, string username, string password)
        {
            string connString = @"Data Source = " + datasource + "; Initial Catalog = " + database 
                + "; Integrated Security = True; User ID = " + username + "; Password = " + password;

            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
    }
}
