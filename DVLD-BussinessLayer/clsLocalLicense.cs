using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BussinessLayer
{
    public class clsLocalLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID {  get; set; }
        public int DriverID {  get; set; }

        public int LicenseClass {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public double PaidFees {  get; set; }
        public bool IsActive {  get; set; }

        public byte IssueReason {  get; set; }
        public int UserID { get; set; }

        public clsLocalLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = string.Empty;
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = 1;
            this.UserID = -1;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static DataTable GetLocalLicensesHistory(int PersonID)
        {
            DataTable LicensesList = clsLocalLicenseData.ListLicenses(PersonID);
            _AssignIsSystemErrorVariable();
            return LicensesList;
        }
        public static bool IsExpired(int LicenseID)
        {
            bool IsExpired = clsLocalLicenseData.IsExpired(LicenseID);
            _AssignIsSystemErrorVariable();
            return IsExpired;
        }
        public static int GetDriverID(int LicenseID)
        {
            return clsLocalLicenseData.GetDriverID(LicenseID);
        }
        public static bool DeActive(int LicenseID)
        {
            bool IsDeActivated = clsLocalLicenseData.DeActive(LicenseID);
            _AssignIsSystemErrorVariable();
            return IsDeActivated;
        }
        public static int IsDriverHasAnActiveLicenseInThisClass(int DriverID,int LicenseClassID,int CurrentLicenseID)
        {
            int LicenseID = clsLocalLicenseData.IsDriverHasAnActiveLicenseInThisClass(DriverID, LicenseClassID,CurrentLicenseID);
            _AssignIsSystemErrorVariable();
            return LicenseID;
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLocalLicenseData.AddNewLicenseandGetID(this.ApplicationID, this.DriverID, this.LicenseClass,
                this.IssueDate, this.ExpirationDate, this.Notes,this.PaidFees, this.IsActive, this.IssueReason, this.UserID);
            return LicenseID != -1;
        }
        public bool Save()
        {
            if (_AddNewLicense())
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
