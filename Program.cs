using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKGL;
using System.Runtime.Serialization.Json;

using System.Data.SQLite;
using System.IO;
using System.Data;

namespace TestHTTP
{

    public class Result
    {
        public string _id { get; set; }
        public int CustomerId { get; set; }
        public int __v { get; set; }
        public string target { get; set; }
        public string Desp { get; set; }
        public string ValidTime { get; set; }
        public string MachineStatus { get; set; }
        public string LastLogTime { get; set; }
        public string CreationDate { get; set; }
        public string CDKey { get; set; }
        public string MachineCode { get; set; }
        public string CustomerName { get; set; }
    }

    public class RootObject
    {
        public List<Result> result { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //TestKeyMethod();

            //TestHttpGet();
            //TestHttpPost();
            TestSqlite();

        }

        private static void TestSqlite()
        {
            string filename = Environment.GetEnvironmentVariable("localappdata") + @"\CamAligner\Support\Data\Vehicle.db";

            if (File.Exists(filename))
            {
                var conn = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
                const string sql = "select * from Vehicle;";


                try
                {
                    Console.WriteLine(conn.State);

                    conn.Open();
                    DataSet ds = new DataSet();
                    var da = new SQLiteDataAdapter(sql, conn);
                    da.Fill(ds);

                    foreach (var column in ds.Tables[0].Columns)
                    {
                        Console.WriteLine(column.ToString());
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
                Console.ReadKey();

            }
        }

        private static void TestKeyMethod()
        {
            SerialKeyConfiguration skc = new SerialKeyConfiguration();
            Validate validate = new Validate(skc);
            //CDKey: KTZEY - UBGPZ - REXIG - INWXB, MachineCode: 92040
            validate.Key = "KTZEY - UBGPZ - REXIG - INWX";
            validate.secretPhase = "hello";
        }

        private static void TestHttpPost()
        {
            throw new NotImplementedException();
        }

        private static void TestHttpGet()
        {
            //Console.WriteLine(DateTime.Now.ToShortDateString());
            // format like: 2017-07-30T16:56:25
            //Console.WriteLine(DateTime.Now.ToString("s") + "Z");
            var customerData = GetUrltoHtml("http://nirovm2-pc:3000/GetCustomerInfo?MachineCode=92040");
            if (customerData.result.Count == 0)
            {
                Console.WriteLine("Not Uploaded");
                GetUrltoHtml("http://nirovm2-pc:3000/UploadCustomerInfo?MachineCode=92040&CDKey=KTZEY-UBGPZ-REXIG-INWXB&CreationDate=2017-06-08&ValidDate=2017-07-08");
            }
            else
            {
                Console.WriteLine(customerData.result[0].LastLogTime);
                DateTime lastLogTime = DateTime.Parse(customerData.result[0].LastLogTime);
                Console.WriteLine((lastLogTime - DateTime.Now).TotalSeconds);

            }

            //Console.WriteLine(GetUrltoHtml("http://nirovm2-pc:3000/GetCustomerInfo?MachineCode=92040", "utf-8"));
            Console.ReadKey();
        }

        public static RootObject GetUrltoHtml(string Url)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                using (System.IO.Stream respStream = wResp.GetResponseStream())
                {
                    DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(RootObject));
                    return (RootObject)deseralizer.ReadObject(respStream);// //反序列化ReadObject
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(String.Format("Get Web Data Message {0}", ex.Message));
            }
            return null;
        }

        public static string GetUrltoHtml(string Url, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();

                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(RootObject));
                RootObject model = (RootObject)deseralizer.ReadObject(respStream);// //反序列化ReadObject

                foreach (var row in model.result)
                {
                    Console.WriteLine(row.CreationDate.GetType().Name + " : " + row.CreationDate);
                }
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (var reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }


            }
            catch (System.Exception ex)
            {
                //errorMsg = ex.Message;
            }
            return "";
        }
    }
}
