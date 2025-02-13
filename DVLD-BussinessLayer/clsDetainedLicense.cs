using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BussinessLayer
{
    public class clsDetainedLicense
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public double FineFees { get; set; }
        public int CreatedUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }

        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;
        public clsDetainedLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            FineFees = 0;
            CreatedUserID = -1;
            Mode = enMode.AddNew;
        }

        public clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, double fineFees, int createdUserID, bool isReleased, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedUserID = createdUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;
            Mode = enMode.Update;
        }
        static DataTable _dt = new DataTable();
        private bool _AddNewDetainedLicense()
        {
            DetainID = clsDetainedLicenseData.AddNewDetainedLicenseAndGetID(this.LicenseID, this.DetainDate
                , this.FineFees, this.CreatedUserID);
            return DetainID != -1;
        }
        public static clsGlobal.stDetainedLicenseInfo GetDetainedLicense(int LicenseID)
        {
            int DetainedID = -1, CreatedByUserID = -1;
            DateTime DetainDate = DateTime.Now;
            double FineFees = 0;
            if (clsDetainedLicenseData.GetDetainedLicense(LicenseID,
                ref DetainedID, ref CreatedByUserID, ref DetainDate, ref FineFees))
                return new clsGlobal.stDetainedLicenseInfo(DetainedID, DetainDate, FineFees, CreatedByUserID);
            else return new clsGlobal.stDetainedLicenseInfo();
        }

        public static bool Release(int DetainID,int ReleasedByUserID,int ReleasedApplicationID)
        {
            return clsDetainedLicenseData.Release(DetainID,ReleasedByUserID, ReleasedApplicationID);
        }
        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static DataTable ListDetainedLicenses()
        {
            _dt = clsDetainedLicenseData.ListDetainedLicenses();
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable Filter(string FilterType, string FilterText)
        {
            if (FilterText != "")
            {
                switch (FilterType)
                {
                    case "None":
                        return ListDetainedLicenses();
                    case "Detain ID":
                        if (int.TryParse(FilterText, out int ID))
                            return FilterByDetainID(ID);
                        else
                        {
                            clsGlobal.IsValidID = false;
                            return FilterByDetainID(-1);

                        }
                    case "Released Application ID":
                        if (int.TryParse(FilterText, out ID))
                            return FilterByApplicationID(ID);
                        else
                        {
                            clsGlobal.IsValidID = false;
                            return FilterByApplicationID(-1);

                        }
                    case "Full Name":
                        return FilterByFullName(FilterText);
                    case "National No":
                        return FilterByNationalNumber(FilterText);
                    default:
                        if (FilterText == "All")
                            return ListDetainedLicenses();
                        return FilterByIsReleased(FilterText);
                }
            }
            else
            {
                return ListDetainedLicenses();
            }
        }
        public static DataTable FilterByFullName(string FilterText)
        {
            _dt = clsDetainedLicenseData.FilterByFullName(FilterText);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByIsReleased(string FilterText)
        {
            _dt = clsDetainedLicenseData.FilterByIsReleased(FilterText);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByApplicationID(int ID)
        {
            _dt = clsDetainedLicenseData.FilterByApplicationID(ID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByDetainID(int ID)
        {
            _dt = clsDetainedLicenseData.FilterByDetainID(ID);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static DataTable FilterByNationalNumber(string NationalNumber)
        {
            _dt = clsDetainedLicenseData.FilterByNationalNumber(NationalNumber);
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                   // return _UpdateDetainedLicense();

                default:
                    return false;
            }
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }
    }
}
