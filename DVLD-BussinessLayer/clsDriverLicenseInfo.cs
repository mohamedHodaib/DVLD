using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BussinessLayer
{
    public  class clsDriverLicenseInfo
    {
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        public int LicenseID {  get; set; }
        public int DriverID {  get; set; }
        public int LicenseClass {  get; set; }
        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }
        public string Notes {  get; set; }
        public bool IsActive {  get; set; } 
        public enIssueReason IssueReason {  get; set; }

        public bool IsDetaind { get {return clsDetainedLicense.IsLicenseDetained(this.LicenseID); } }

        public string IssueReasonText
        {
            get { return GetIssueReasonText(this.IssueReason); }
        }
        public Boolean IsLicenseExpired()
        {

            return (this.ExpirationDate < DateTime.Now);

        }
        public clsDriver DriverInfo { get; set; }

      public   clsDetainedLicense DetainedLicense = new clsDetainedLicense();
        public clsDriverLicenseInfo()
        {
            LicenseID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = string.Empty;
            IsActive = true;
            IssueReason = enIssueReason.FirstTime;

        }

        public clsDriverLicenseInfo(int LicenseID,int DriverID,int LicenseClass,DateTime IssueDate,
            DateTime ExpirationDate,string Notes,bool IsActive,enIssueReason IssueReason)
        {
            this.LicenseID = LicenseID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;


            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
        }
        public static clsDriverLicenseInfo GetDriverLicenseInfo(int LocalLicenseID)
        {
            int  DriverID = -1, LicenseClass = -1;
            string Notes = string.Empty;
            byte IssueReason = 1;
            DateTime IssueDate = DateTime.Now,ExpirationDate = DateTime.Now;
            bool IsActive = true;
            if (clsDriverLicenseInfoData.FindLicenseInfo( LocalLicenseID,ref DriverID,
                ref IssueDate,ref ExpirationDate,ref Notes,ref IsActive,ref IssueReason,ref LicenseClass))
            {
                if (ExpirationDate <= DateTime.Now)
                    IsActive = false;
                return new clsDriverLicenseInfo(LocalLicenseID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                    Notes, IsActive,(enIssueReason) IssueReason);
            }
            return null;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }

        public static clsDriverLicenseInfo GetActiveDriverLicenseWithClass3Info(int LicenseID)
        {
            int DriverID = -1, LicenseClass = -1;
            string Notes = string.Empty;
            byte IssueReason = 1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true, IsDetained = false;
            bool IsFound = clsDriverLicenseInfoData.GetActiveDriverLicenseWithClass3Info( LicenseID, ref DriverID,
                ref IssueDate, ref ExpirationDate, ref Notes, ref IsActive, ref IssueReason, ref LicenseClass, ref IsDetained);
            _AssignIsSystemErrorVariable();
            if (IsFound)
            {
                if (ExpirationDate <= DateTime.Now)
                    IsActive = false;

                return new clsDriverLicenseInfo(LicenseID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                    Notes, IsActive,(enIssueReason) IssueReason);
            }
            return null;
        }
        public static clsDriverLicenseInfo GetDriverLicenseInfoWithLicenseID(int LicenseID)
        {
            int DriverID = -1, LicenseClass = -1;
            string Notes = string.Empty;
            byte  IssueReason = 1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;
            bool IsFound = clsDriverLicenseInfoData.FindLicenseInfo(LicenseID, ref DriverID,
                ref IssueDate, ref ExpirationDate, ref Notes, ref IsActive, ref IssueReason, ref LicenseClass);
            if (IsFound)
            {

                return new clsDriverLicenseInfo(LicenseID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                    Notes, IsActive,(enIssueReason) IssueReason);
            }
            return null;
        }
        public static bool IsLicenseExistByPersonID(int PersonID,int LicenseClassID)
        {
            return GetActiveLicenseIDPersonID(PersonID, LicenseClassID) != -1;
        }

        public static int GetActiveLicenseIDPersonID(int PersonID,int LicenseClassID)
        {
            return clsDriverLicenseInfoData.GetActiveLicenseIDPersonID(PersonID , LicenseClassID);  
        }
        public bool Detain(double FineFees)
        {
        
            DetainedLicense.LicenseID = LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = FineFees;
            DetainedLicense.CreatedUserID = clsGlobal.CurrentUser.UserID;
            return DetainedLicense.Save();
        }
        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }
    }
}
