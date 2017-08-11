using AppController;
using AppController.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHTTP
{
    class SQLiteController
    {
        static SQLiteService mySQLiteService;

        public static SQLiteService MySQLiteService
        {
            get
            {
                if (mySQLiteService == null)
                {
                    SQLiteConnection conn = SQLiteUtil.ConnectToDB(SQLiteUtil.GetVehiecleDBPath());
                    if (conn.State == ConnectionState.Open)
                    {
                        mySQLiteService = new SQLiteService();
                        mySQLiteService.Conn = conn;
                    }
                    else
                    {
                        mySQLiteService = null;
                    }

                }
                return mySQLiteService;
            }
        }
    }
}
