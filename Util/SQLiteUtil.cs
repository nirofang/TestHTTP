using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace AppController.Util
{
    class SQLiteUtil
    {

        internal static string GetVehiecleDBPath()
        {
            return Environment.GetEnvironmentVariable("localappdata") + @"\CamAligner\Support\Data\Vehicle.db";
        }

        internal static SQLiteConnection ConnectToDB(string filename)
        {
            SQLiteConnection conn = null;
            if (File.Exists(filename))
            {
                conn = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
                conn.Open();
            }
            else
            {
                throw new FileNotFoundException("Cannnot find the file: ", filename);
            }
            return conn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql">""select * from Vehicle;";"</param>
        /// <returns></returns>
        internal static string GetTableFromSqlite(SQLiteConnection conn, string sql)
        {
            try
            {
                DataSet ds = new DataSet();
                var da = new SQLiteDataAdapter(sql, conn);
                da.Fill(ds);
                return ds2json(ds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ds2json(DataSet ds)
        {
            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }
    }
}
