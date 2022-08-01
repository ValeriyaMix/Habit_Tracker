using System;
using Microsoft.Data.Sqlite;

namespace coding_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            TrackerFunctionality objTracker = new TrackerFunctionality();
            string connectionString = @"Data Source=Coding-Tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableHabit = connection.CreateCommand();

                tableHabit.CommandText = 
                @"CREATE TABLE IF NOT EXISTS programming_tracker (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Minutes INTEGER
                    )";

                tableHabit.ExecuteNonQuery();

                connection.Close();
            }

            

            bool closeTracker = false;

            while(closeTracker == false)
            {
                string commandUser = objTracker.GetUserInput();

                switch (commandUser)
                {
                    case "0":
                        Console.WriteLine("\nSee you later\n");
                        closeTracker = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        objTracker.GetAllRecords(connectionString);
                        break;
                    case "2":
                        objTracker.Insert(connectionString);
                        break;
                    case "3":
                        //objTracker.Delete();
                        break;
                    case "4":
                        //objTracker.Update();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
            
            Console.ReadKey();
        }
    }
}

