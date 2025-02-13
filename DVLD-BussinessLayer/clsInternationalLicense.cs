using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BussinessLayer
{
    public class clsInternationalLicense
    {
        public int ILID {  get; set; }
        public int ApplicationID {  get; set; } 
        public int DriverID { get; set; }
        public int LLID {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive {  get; set; }
        public int UserID { get; set; }

     static    DataTable _dt = new DataTable();
        public clsInternationalLicense()
        {
            this.ILID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LLID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;
            this.UserID = -1;
        }

        public clsInternationalLicense(int iLID, int applicationID, int driverID, 
            int lLID, DateTime issueDate, DateTime expirationDate, bool isActive, int userID)
        {
            ILID = iLID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LLID = lLID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            UserID = userID;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }

        public static DataTable Filter(string FilterType, string FilterText)
        {
            if (FilterText != "")
            {
                switch (FilterType)
                {
                    case "None":
                        return ListInternationalLicenses();
                    case "Int License ID":
                        if (int.TryParse(FilterText, out int ID))
                            return FilterByILID(ID);
                        else
                        {
                            clsGlobal.IsValidID = false;
                            return FilterByILID(-1);

                        }
                    case "L License ID":
                        if (int.TryParse(FilterText, out ID))
                            return FilterByLLID(ID);
                        else
                        {
                            clsGlobal.IsValidID = false;
                            return FilterByLLID(-1);

                        }
                    case "Application ID":
                        if (int.TryParse(FilterText, out ID))
                            return FilterByAppID(ID);
                        else
                        {
                            clsGlobal.IsValidID = false;
                            return FilterByAppID(-1);

                        }
                    case "Driver ID":
                        if (int.TryParse(FilterText, out ID))
                            return FilterByDriverID(ID);
                        else
                        {
                            clsGlobal.IsValidID = false;
                            return FilterByDriverID(-1);

                        }
                    default:
                        if (FilterText == "All")
                            return ListInternationalLicenses();
                        return FilterByActivation(FilterText);
                }
            }
            else
            {
                return ListInternationalLicenses();
            }
        }

        public static DataTable FilterByActivation(string FilterText)
        {
            _dt = clsInternationalLicenseData.FilterByActivation(FilterText);
            _AssignIsSystemErrorVariable();
            return _dt;
        }

        public static DataTable FilterByLLID(int ID)
        {
            _dt = clsInternationalLicenseData.FilterByLLID(ID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByAppID(int AppID)
        {
            _dt = clsInternationalLicenseData.FilterByAppID(AppID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByILID(int ID)
        {
            _dt = clsInternationalLicenseData.FilterByILID(ID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByDriverID(int ID)
        {
            _dt = clsInternationalLicenseData.FilterByDriverID(ID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable GetInternationalLicensesHistory(int PersonID)
        {
            DataTable LicensesList = clsInternationalLicenseData.ListPersonLicenses(PersonID);
            _AssignIsSystemErrorVariable();
            return LicensesList;
        }
        public static int FindInternationalLicense(int LLicenseID)
        {
          int ILID = clsInternationalLicenseData.FindInternationalLicense(LLicenseID);
            _AssignIsSystemErrorVariable();
            return ILID;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {

            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);

        }
        public static  DataTable ListInternationalLicenses()
        {
           _dt = clsInternationalLicenseData.ListAllLicenses();
            _AssignIsSystemErrorVariable();
            return _dt;
        }

        public static int GetPersonIDByILID(int ILID)
        {   
            int PersonID = clsInternationalLicenseData.GetPersonIDByILID(ILID); 
            _AssignIsSystemErrorVariable();
            return PersonID;
        }
        private bool _AddNewIL()
        {
            this.ILID = clsInternationalLicenseData.AddNewIL(this.ApplicationID,this.DriverID,
                this.LLID, this.IssueDate,this.ExpirationDate,this.IsActive,this.UserID);
            return this.ILID != -1;
        }
        
        public bool Save()
        {
           if( _AddNewIL())
                return true;
           else return false;
        }
    }
}
