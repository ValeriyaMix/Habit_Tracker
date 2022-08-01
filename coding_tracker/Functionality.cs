using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace coding_tracker
{
    public class TrackerFunctionality
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
                int idProgramming = reader.GetInt32(0);
                DateTime dateProgramming = DateTime.ParseExact(reader.GetString(1), "dd/MM/yy", new CultureInfo("en-US"));
                int minutesProgramming = reader.GetInt32(2);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //tableData.Add(new Programming(idProgramming, dateProgramming, minutesProgramming));
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
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"Number of a row: {dw.Id} - Date of a programming: {dw.Date.ToString("dd/MM/yy")} - Minutes: {dw.Minutes}");
                    //Console.WriteLine(dw);
                }
                Console.WriteLine("------------------------------------------\n");
            }
        }

        public string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd/mm/yy). Type 0 to return to main manu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            // while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            // {
            //     Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
            //     dateInput = Console.ReadLine();
            // }

            return dateInput;
        }

        public int GetNumberInput()
        {
            Console.WriteLine("\n\nPlease insert how many minutes you spent programming. Type 0 to return to main manu.\n\n");

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            // while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            // {
            //     Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            //     numberInput = Console.ReadLine();
            // }

            int integerInput = Convert.ToInt32(numberInput);

            return integerInput;
        }

    }

    
}