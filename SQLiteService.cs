using AppController.Util;
using System.Data.SQLite;
using System.Data;

namespace AppController
{
    internal class SQLiteService
    {
        SQLiteConnection conn;

        internal SQLiteConnection Conn {
            get { return conn; }
            set { conn = value; }
        }

        internal string GetJsonOfSQL(string sql)
        {
            return SQLiteUtil.GetTableFromSqlite(conn, sql);
        }
    }
}