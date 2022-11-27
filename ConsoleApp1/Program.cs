using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        //static void Main(string[] args)
        //{
        //    string commandText = "WAITFOR DELAY '00:00:10';" +
        //        "SELECT * FROM Authors";
        //    RunCommandAsync(commandText, ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString);


        //}

        //private static void RunCommandAsync(string commandText, string connectionString)
        //{
        //    SqlConnection connection=new SqlConnection(connectionString);
        //    SqlCommand command=new SqlCommand(commandText, connection);
        //    connection.Open();

        //    IAsyncResult result = command.BeginExecuteReader(CommandBehavior.CloseConnection);


        //    //int count = 0;
        //    string text = "Loading ";

        //    while (!result.IsCompleted)
        //    {
        //        Console.ReadLine();

        //        //Console.WriteLine(text);
        //        //Thread.Sleep(300);
        //        //Console.Clear();
        //        //text += " .";

        //        //if(text.Contains(" . . . "))
        //        //{
        //        //    text = "Loading ";
        //        //}
        //    }


        //    using (var reader=command.EndExecuteReader(result))
        //    {
        //        DisplayResults(reader);
        //    }

        //}


        static void Main(string[] args)
        {
            string commandText = "WAITFOR DELAY '00:00:03';" +
    "SELECT * FROM Authors";
            RunCommandAsync(commandText, ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString);

        }

        private static void RunCommandAsync(string commandText, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(commandText, connection);
            connection.Open();

            // IAsyncResult result = command.BeginExecuteReader(new AsyncCallback(GetDataCallback),command);


            //int count = 0;
            // string text = "Loading ";




            #region Wait Handle

            var result = command.BeginExecuteReader();
            //string text = "Loading ";
            //while (!result.IsCompleted)
            //{
            //    Console.WriteLine(text);
            //    Thread.Sleep(300);
            //    Console.Clear();
            //    text += " .";

            //    if (text.Contains(" . . . "))
            //    {
            //        text = "Loading ";
            //    }
            //}

        

            WaitHandle handle = result.AsyncWaitHandle;
            if (handle.WaitOne(TimeSpan.FromSeconds(4)))
            {
                GetData(command, result);
            }

            #endregion


        }

        private static void GetData(SqlCommand command, IAsyncResult result)
        {
            using (var reader = command.EndExecuteReader(result))
            {
                DisplayResults(reader);
            }
        }

        private static void GetDataCallback(IAsyncResult result)
        {
            var command = (SqlCommand)result.AsyncState;
            using (var reader = command.EndExecuteReader(result))
            {
                DisplayResults(reader);
            }
        }

        private static void DisplayResults(SqlDataReader reader)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.WriteLine($"{reader.GetValue(i)}\t");
                }
            }
        }
    }
}
