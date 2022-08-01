using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker
{
    internal interface ITrackerFunctionality
    {
        void GetUserInput();

        void Insert();

        string GetDateInput();

        int GetNumberInput();

        void GetAllRecords();

        void Delete();

        void Update();
    }
}