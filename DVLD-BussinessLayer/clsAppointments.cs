using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BussinessLayer
{
    public class clsAppointments
    {
        static DataTable _dt = new DataTable();
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;
        public  int TestAppointmentID {  get; set; }
        public clsTestTypes.enTestType TestTypeID {  get; set; }
        public  int LocalDrivingLicenseApplicationID {  get; set; }
        public  DateTime AppointmentDate { get; set; }
        public double PaidFees { get; set; }
        public int UserID {  get; set; }    
        public bool IsLocked {  get; set; }

        public int RetakeTestApplicationID {  get; set; }

        public clsApplication RetakeTestApplication { get; set; }

        public clsAppointments()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestTypes.enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.UserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;
            Mode = enMode.AddNew;
        }

        public clsAppointments(int TestAppointmentID, clsTestTypes.enTestType TestTypeID,
          int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
          int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)

        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.UserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestApplication = clsApplication.FindApplicationByID(RetakeTestApplicationID);
            Mode = enMode.Update;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static DataTable GetAppointments(int LDLAppID,int TestTypeID)
        {
          _dt =  clsAppointmentsData.GetAppointments(LDLAppID,TestTypeID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static bool LockTheAppointment(int AppointmentID)
        {
           return clsAppointmentsData.LockTheAppointment(AppointmentID);
        }
        public static DateTime GetAppointmentDate(int AppointmentID)
        {
            DateTime AppointmentDate = clsAppointmentsData.GetAppointmentDate(AppointmentID);
            _AssignIsSystemErrorVariable();
            return AppointmentDate;
        }
        public static bool IsHaveAnActiveAppointment(int LDLAppID,clsTestTypes.enTestType TestType)
        {
            bool IsHaveAnActiveAppointment = clsAppointmentsData.IsHaveAnActiveAppointment(LDLAppID,(int) TestType);
            _AssignIsSystemErrorVariable();
            return IsHaveAnActiveAppointment;
        }

        

        public static bool IsAppointmentLocked(int AppointmentID)
        {
            bool IsLocked = clsAppointmentsData.IsAppointmentLocked(AppointmentID);
            _AssignIsSystemErrorVariable();
            return IsLocked;
        }
        public static clsAppointments Find(int TestAppointmentID)
        {
            int TestTypeID = 1; int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int CreatedByUserID = -1; bool IsLocked = false; int RetakeTestApplicationID = -1;

            if (clsAppointmentsData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
            ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsAppointments(TestAppointmentID, (clsTestTypes.enTestType)TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;

        }
        private bool _AddNewAppointment()
        {
            this.TestAppointmentID = clsAppointmentsData.AddNewAppointmentandGetID((int)this.TestTypeID
                ,this.LocalDrivingLicenseApplicationID,this.AppointmentDate,this.PaidFees
                ,this.UserID,this.RetakeTestApplicationID);
            return TestAppointmentID != -1;
        }
        private bool _UpdateAppointment()
        {
            return clsAppointmentsData.UpdateAppointment(this.TestAppointmentID, this.AppointmentDate);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateAppointment();

                default:
                    return false;
            }
        }
    }
}
