using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BussinessLayer
{
    public class clsGlobal
    {
        public static bool IsSystemError = false;
        public static bool IsForeignKeyConstraintException = false;
        public static bool IsActive = true;
        public static bool IsValidID = true;
        public static bool IsPersonInformationFillThePersonCard = false;
        public static int CurrentUserID = 0;    
        public static clsUser CurrentUser = new clsUser();
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };
        public static  clsApplication _App = new clsApplication();

        public static clsLocalDrivingLicenseApplication LDLApp = new clsLocalDrivingLicenseApplication();
        public static clsApplication App = new clsApplication();
        public static void MakeIsPersonFillThePersonCardFalse()
        {
            IsPersonInformationFillThePersonCard = false;
        }

        public  struct stDetainedLicenseInfo
        {
            public stDetainedLicenseInfo(int DetainedID,DateTime DetainedDate,
                double FineFees,int CreatedByUserID)
            {
                this.DetainedID = DetainedID;
                this.CreatedByUserID = CreatedByUserID;
                this.FineFees = FineFees;
                this.DetainDate = DetainedDate;
            }
            public int DetainedID { get; set; }
            public DateTime DetainDate { get; set; }
            public double FineFees { get; set; }
            public int CreatedByUserID { get; set; }
        }
    }
}
