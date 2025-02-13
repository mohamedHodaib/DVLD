using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BussinessLayer
{
    public  class clsDriverInternationalLicenseInfo
    {
        public int ILicenseID {  get; set; }
        public int LLicenseID { get; set; }
        public int DriverID { get; set; }
        public int ApplicationID {  get; set; } 
        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public clsDriver DriverInfo { get; set; }

        public clsDriverInternationalLicenseInfo()
        {
            ILicenseID = -1;
            LLicenseID = -1;
            DriverID = -1;
            ApplicationID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = true;
        }

        public clsDriverInternationalLicenseInfo(int ILicenseID,int LLicenseID, int DriverID,int ApplicationID, DateTime IssueDate,
            DateTime ExpirationDate, bool IsActive)
        {
            this.ILicenseID = ILicenseID;
            this.LLicenseID = LLicenseID;
            this.DriverID = DriverID;
            this.ApplicationID = ApplicationID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
        }
        public static clsPerson P = new clsPerson();
        public static clsDriverInternationalLicenseInfo GetDriverInternationalLicenseInfo(int ILID)
        {
           
               int PersonID = clsInternationalLicense.GetPersonIDByILID(ILID);
                P = clsPerson.FindByPersonID(PersonID).Result;
            int DriverID = -1,LLID = -1,ApplicationID = -1;
            string  ImagePath = P.ImagePath, Name = P.FName + " " + P.SName + " " + P.TName + " " + P.LName,
                NationalNo = P.NationalNo;
            short Gender = P.Gender;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now, DateOfBirth = P.DateOfBirth;
            bool IsActive = true;
            if (clsDriverInternationalLicenseInfoData.FindLicenseInfo(ILID,ref LLID,  ref DriverID,ref ApplicationID,
                ref IssueDate, ref ExpirationDate, ref IsActive))
            {
                return new clsDriverInternationalLicenseInfo(ILID,LLID, DriverID,ApplicationID, IssueDate, ExpirationDate,
                    IsActive);
            }
            return null;
        }

     
        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }

    }
}
