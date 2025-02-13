using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public class clsValidations
    {
        public static bool  ValidateInteger(string Number)
        {
            return Regex.IsMatch(Number, @"^[0-9]*$");
        }

        public static bool ValidateFloat(string Number)
        {
            return Regex.IsMatch(Number, @"^[0-9]*(?:\.[0-9]*)?$");
        }
        public static bool IsNumber(string Number)
        {
            return ValidateInteger(Number)  || ValidateFloat(Number);
        }

        public static void LogTheError(string Message, EventLogEntryType eventType)
        {
            if(!EventLog.SourceExists("DVLD"))
            {
                EventLog.CreateEventSource("DVLD", "Application");
            }

            EventLog.WriteEntry("DVLD", Message, eventType);
        }
    }
}
