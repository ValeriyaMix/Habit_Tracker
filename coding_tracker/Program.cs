using System;
using Microsoft.Data.Sqlite;

namespace coding_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=Coding-Tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableHabit = connection.CreateCommand();

                tableHabit.CommandText = 
                @"CREATE TABLE IF NOT EXISTS programming_tracking (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Hours INTEGER
                    )";

                tableHabit.ExecuteNonQuery();

                connection.Close();
            }
            
            Console.ReadKey();
        }
    }
}

