using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_BussinessLayer.clsGlobal;

namespace DVLD_BussinessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public clsApplicationType ApplicationTypeInfo { get;set;}
        public clsGlobal.enApplicationStatus ApplicationStatus {  get; set; }
        public DateTime LastStatusDate { get; set; }
        public double PaidFees {  get; set; }
        public int UserID { get; set; }

        public clsUser CreatedByUserInfo { get; set; }

        public string StatusText
        {
            get
            {

                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = clsGlobal.enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.UserID = -1;
            Mode = enMode.AddNew;
        }


        public clsApplication(int appID, int applicantPersonID,DateTime applicationDate,int applicantTypeID
            ,byte applicationStatus,DateTime LastStatusDate,double PaidFees,int UserID)
        {
            this.ApplicationID = appID;
            this.ApplicantPersonID= applicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicantTypeID;
            this.ApplicationTypeInfo = clsApplicationType.GetApplicationType(this.ApplicationTypeID);
            this.ApplicationStatus = (clsGlobal.enApplicationStatus)applicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees= PaidFees;
            this.UserID = UserID;
            this.CreatedByUserInfo =  clsUser.FindByUserID(this.UserID).Result;
            Mode = enMode.Update;
        }
        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static int IsPersonAnActiveApplicationInThisClass(int PersonID,enApplicationType ApplicationType,int LicenseClassID)
        {
            int ApplicationID =  clsApplicationData.IsPersonHasLicenseInThisClass(PersonID, (int)ApplicationType,LicenseClassID);
           _AssignIsSystemErrorVariable();
            return ApplicationID;
        }
       
        public static clsApplication FindApplicationByID(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1,UserID = -1;
         byte ApplicationStatus = 1;
            double PaidFees = 0;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;

            if (clsApplicationData.FindApplicationByID(ApplicationID, ref ApplicantPersonID, ref ApplicationTypeID, ref UserID,
                ref ApplicationStatus, ref ApplicationDate, ref LastStatusDate, ref PaidFees))
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate,
                    ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, UserID);
            else
                return null;
        }

        public static clsGlobal.enApplicationStatus GetApplicationStatus(int LDLAppID)
        {
            clsGlobal.enApplicationStatus status =(clsGlobal.enApplicationStatus)
                clsApplicationData.GetApplicationStatus(LDLAppID);
            _AssignIsSystemErrorVariable();
            return status;
        }

        public  bool Delete()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }

        public static int GetPersonID(int AppID)
        {
            return clsApplicationData.GetPersonID(AppID);
        }
        public  bool Cancel()
        {
            return clsApplicationData.UpdateApplicationStatus(ApplicationID, DateTime.Now, 2);
        }
        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplicationandGetID(this.ApplicantPersonID, this.ApplicationDate,
               this.ApplicationTypeID, (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.UserID);
            return ApplicationID != -1;
        }
        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplicationStatus(this.ApplicationID, this.LastStatusDate,Convert.ToByte
                (this.ApplicationStatus));
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
