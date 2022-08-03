using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker
{
    internal interface ITrackerFunctionality
    {
        string GetUserInput();

        void Insert(string connectionString);

        string GetDateInput();

        int GetNumberInput();

        void GetAllRecords(string connectionString);

        void Delete(string connectionString);

        void Update(string connectionString);
    }
}