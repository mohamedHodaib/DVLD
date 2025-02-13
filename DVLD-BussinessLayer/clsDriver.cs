using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BussinessLayer
{
    public  class clsDriver
    {
        public int DriverID { get; set; }
        public int PersonID {  get; set; }
        public int UserID {  get; set; }
        public DateTime CreatedDate { get; set; }


     static   DataTable dt = new DataTable();  
        public clsPerson PersonInfo { get; set; }

        public clsDriver ()
        {
            DriverID = -1;
            PersonID = -1;
            UserID = -1;
            CreatedDate = DateTime.Now;
        }
        public clsDriver(int driverID, int personID, int userID, DateTime createdDate)
        {
            DriverID = driverID;
            PersonID = personID;
            UserID = userID;
            CreatedDate = createdDate;
            PersonInfo = clsPerson.FindByPersonID(PersonID).Result;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static bool IsDriver(int PersonID)
        {
            bool IsDriver = clsDriverData.IsDriver(PersonID);
            _AssignIsSystemErrorVariable();
            return IsDriver;
        }

        public static int GetDriverID(int PersonID)
        {
           int DriverID = clsDriverData.GetDriverID(PersonID);
            _AssignIsSystemErrorVariable();
            return DriverID;
        }
        public static DataTable ListDrivers()
        {
            dt = clsDriverData.ListDrivers();
            _AssignIsSystemErrorVariable ();
            return dt;
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            int PersonID = -1; int CreatedByUserID = -1; DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID,
                ref CreatedByUserID, ref CreatedDate))
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }
        public static int GetPersonID(int DriverID)
        {
            int PersonID = clsDriverData.GetPersonIDByDriverID(DriverID);
            _AssignIsSystemErrorVariable();
            return PersonID;
        }
        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriverandGetID(this.PersonID, this.UserID,
                this.CreatedDate);
            return DriverID != -1;
        }

        public bool Save()
        {
            if (_AddNewDriver())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
