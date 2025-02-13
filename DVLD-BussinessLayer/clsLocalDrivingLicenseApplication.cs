using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace DVLD_BussinessLayer
{
    public class clsLocalDrivingLicenseApplication 
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        private static DataTable _dt;
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.ApplicationID = -1;
           this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }


        public clsLocalDrivingLicenseApplication(int LDLapp,int appID,int licenseClassID)
        {
            this.LocalDrivingLicenseApplicationID=LDLapp;
            this.ApplicationID = appID;
            this.LicenseClassID = licenseClassID;
            Mode = enMode.Update;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static DataTable ListLocalDrivingLicenseApplications()
        {
            _dt = clsLocalDrivingLicenseApplicationData.ListLocalDrivingLicenseApplications();
            _AssignIsSystemErrorVariable();
            return _dt;
        }

        public static int GetPersonIDByLDLAppID(int LDLAppID)
        {
            return clsLocalDrivingLicenseApplicationData.GetPersonIDByLDLAppID(LDLAppID);
        }
        public static int GetLocalLicenseIDByLDLAppID(int LDLAppID)
        {
           return clsLocalDrivingLicenseApplicationData.GetLocalLicenseIDByLDLAppID(LDLAppID);
        }
        public static clsLocalDrivingLicenseApplication FindLDLApplicationByID(int ID)
        {
            int ApplicationID = -1,LicenseClassID = -1;
            if ( clsLocalDrivingLicenseApplicationData.FindLDLApplicationByID(ID))
                return new clsLocalDrivingLicenseApplication(ID, ApplicationID,LicenseClassID);
            else
                return null;
        }

        public short GetPassedTests()
        {
            return clsTests.GetPassedTests(this.LocalDrivingLicenseApplicationID);
        }
        public bool DoesAttendTestType(clsTestTypes.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public  bool Delete()
        {
            return clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);
        }
        public static bool IsPassedThisTestType(int LDLAppID, clsTestTypes.enTestType TestType)
        {
            bool IsPassedThisTestType = clsLocalDrivingLicenseApplicationData.IsPassedThisTestType(LDLAppID,(int) TestType);
            _AssignIsSystemErrorVariable();
            return IsPassedThisTestType;
        }
        private bool _AddNewApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewApplicationandGetID(this.ApplicationID,this.LicenseClassID);
            return LocalDrivingLicenseApplicationID != -1;
        }
        private bool _UpdateApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateApplication(this.LocalDrivingLicenseApplicationID, this.LicenseClassID);
        }

        public static int GetApplicationID(int LDLAppID)
        {
          return clsLocalDrivingLicenseApplicationData.GetApplicationID(LDLAppID);  
        }
        public bool Save()
        {
            
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateApplication();

                default:
                    return false;
            }
        }
    }
}
