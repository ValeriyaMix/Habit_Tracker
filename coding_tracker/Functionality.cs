using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace coding_tracker
{
    public class TrackerFunctionality: ITrackerFunctionality
    {
        
        public string GetUserInput()
        {
            Console.WriteLine(@"
            TRACKER MENU
            What would you like to do?
            Type 0 to close the tracker application
            Type 1 to view all records in the coding tracker database
            Type 2 to insert a new record
            Type 3 to delete the record
            Type 4 to update the record
            -----------------------------------------------------------
            ");

            string userInput = Console.ReadLine();
            return userInput;
        }

        public void Insert(string connectionString)
        {
            string programmingDate = GetDateInput();

            int programmingMinutes = GetNumberInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableHabit = connection.CreateCommand();
                tableHabit.CommandText =
                   $"INSERT INTO programming_tracker(date, minutes) VALUES('{programmingDate}', {programmingMinutes})";

                tableHabit.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void GetAllRecords(string connectionString)
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableHabit = connection.CreateCommand();
                tableHabit.CommandText =
                    $"SELECT * FROM programming_tracker ";

                List<Programming> tableData = new();

                SqliteDataReader reader = tableHabit.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Programming
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd/MM/yy", new CultureInfo("en-US")),
                            Minutes = reader.GetInt32(2)
                        }); ;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("------------------------------------------\n");
                foreach (Programming line in tableData)
                {
                    Console.WriteLine($"{line.Id}) on {line.Date.ToString("dd/MM/yy")} I spent programming {line.Minutes} minutes");  
                }
                Console.WriteLine("------------------------------------------\n");
            }
        }

        public void Delete(string connectionString)
        {
            Console.Clear();
            GetAllRecords(connectionString);

            Console.WriteLine("\n\nPlease type the Id of the record you want to delete.\n\n");
            int userIdInput = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableHabit = connection.CreateCommand();

                tableHabit.CommandText = $"DELETE from programming_tracker WHERE Id = '{userIdInput}'";

                int rowCount = tableHabit.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {userIdInput} doesn't exist. Please press any key to continue" +
                    " deleting or press 0 exit.\n");
                    connection.Close();
                    ConsoleKeyInfo pressedKey = Console.ReadKey();
                    if(pressedKey.Key == ConsoleKey.NumPad0)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Delete(connectionString);
                    }  
                }
                else
                {
                    Console.WriteLine($"\n\nRecord with Id {userIdInput} was deleted. \n\n");
                }

            }

        }

        public void Update(string connectionString)
        {
            GetAllRecords(connectionString);

            Console.WriteLine("\n\nPlease type the Id of the record you want to update.\n\n");
            int userIdInput = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM programming_tracker WHERE Id = {userIdInput})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {userIdInput} doesn't exist.\nPlease press any key to continue" + 
                    " updating or press 0 exit.\n");
                    connection.Close();
                    ConsoleKeyInfo pressedKey = Console.ReadKey();
                    if(pressedKey.Key == ConsoleKey.NumPad0)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Update(connectionString);
                    }
                    
                }
                else
                {
                    string date = GetDateInput();

                    int minutes = GetNumberInput();

                    var tableHabit = connection.CreateCommand();
                    tableHabit.CommandText = $"UPDATE programming_tracker SET date = '{date}', minutes = {minutes} WHERE Id = {userIdInput}";

                    tableHabit.ExecuteNonQuery();

                    connection.Close();
                }

                
            }


        }

        public string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd/mm/yy). Type 0 to return to main manu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd/MM/yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd/mm/yy). Please try again\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        public int GetNumberInput()
        {
            int number;
            Console.WriteLine("\n\nPlease insert how many minutes you spent programming. Type 0 to return to main manu.\n\n");

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            while (!int.TryParse(numberInput, out number) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\nInvalid number. Try again.\n");
                numberInput = Console.ReadLine();
            }

            int integerInput = Convert.ToInt32(numberInput);

            return integerInput;
        }

    }

    
}