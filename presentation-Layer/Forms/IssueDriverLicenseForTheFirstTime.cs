using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Forms
{
    public partial class IssueDriverLicenseForTheFirstTime : Form
    {
        public IssueDriverLicenseForTheFirstTime(int LDLAppID)
        {
            InitializeComponent();
            clsGlobal.LDLApp = clsLocalDrivingLicenseApplication.FindLDLApplicationByID(LDLAppID);
            if (clsGlobal.LDLApp != null)
            {
                clsGlobal.App = clsApplication.FindApplicationByID(clsGlobal.LDLApp.ApplicationID);
                if (clsGlobal.App != null)
                {
                    ctrApplicationInfoCard1.FillTheApplicationFormWithData(clsGlobal.LDLApp, clsGlobal.App);
                }
                else
                    _ShowSystemError();
            }
            else
                _ShowSystemError();
        }

        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);
            this.Close();
        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _FillTheDriverObject(clsDriver Driver)
        {
            Driver.PersonID = clsGlobal.App.ApplicantPersonID;
            Driver.UserID = clsGlobal.CurrentUserID;
        }

        private void _FillLicenseObject(clsLocalLicense License,int DriverID,DateTime IssueDate)
        {
            License.ApplicationID = clsGlobal.App.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = clsGlobal.LDLApp.LicenseClassID;
            License.IssueDate = IssueDate;
            int ValidityYears = clsDrivingLicenseClass.GetValidityYears(License.LicenseClass);
            if (ValidityYears == 0)
                _ShowSystemError();
            License.ExpirationDate = License.IssueDate.AddYears(ValidityYears);
            License.Notes = txtNotes.Text;
            double Fees = clsDrivingLicenseClass.GetFees(License.LicenseClass);
            if (Fees == 0)
                _ShowSystemError();
            License.PaidFees = Fees;
            License.IsActive = true;
            License.IssueReason = Convert.ToByte(clsGlobal.App.ApplicationTypeID);
            License.UserID = clsGlobal.CurrentUserID;
        }

        private void _UpdateApplication()
        {
            clsGlobal.App.LastStatusDate = DateTime.Now;
            clsGlobal.App.ApplicationStatus = clsGlobal.enApplicationStatus.Completed;
            if (!clsGlobal.App.Save())
                _ShowSystemError();
            this.Close();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            int DriverID = -1;
            DateTime IssueDate = DateTime.Now;
            if(!clsDriver.IsDriver(clsGlobal.App.ApplicantPersonID))
            {
                clsDriver Driver = new clsDriver();
                _FillTheDriverObject(Driver);
                if (!Driver.Save())
                    _ShowSystemError();
                DriverID = Driver.DriverID;
                IssueDate = Driver.CreatedDate;
            }
            else
            {
                DriverID = clsDriver.GetDriverID(clsGlobal.App.ApplicantPersonID);
            }

            clsLocalLicense License = new clsLocalLicense();
            _FillLicenseObject(License,DriverID,IssueDate);
           if( License.Save())
            {
                MessageBox.Show("License Issued Successfully with LicenseID = " + License.LicenseID,"Success"
                    ,MessageBoxButtons.OK,MessageBoxIcon.Information);
                clsValidations.LogTheError("License Issued Successfully", EventLogEntryType.Error);
            }
            else  _ShowSystemError();
            _UpdateApplication();
        }
    }
}
