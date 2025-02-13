using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class DataAccessSettings
    {
        public static bool IsForeignKeyConstraintException = false;
        public static bool IsSystemError = false;

        public static void MakeIsSystemErrorFalseBeforeStart()
        {
           IsSystemError = false;
        }
    }
}
