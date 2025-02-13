using DVLD.MyControls;
using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Forms
{
    public partial class NewInternationalLicense : Form
    {
        public NewInternationalLicense()
        {
            InitializeComponent();
        }
        clsDriverLicenseInfo LocaldriverLicenseInfo = new clsDriverLicenseInfo();
        int ILID = -1;
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public  void MakeChangesAfterDriverCardFilled()
        {
            llblShowLicensesHistory.Enabled = true;
            lblLLID.Text = LocaldriverLicenseInfo.LicenseID.ToString();
        }

        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There a system error we will solve try Again Later! ", EventLogEntryType.Error);
            this.Close();
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                clsGlobal.IsSystemError = false;
                _ShowSystemError();
            }
        }
        private void btnGetLicenseInfo_Click(object sender, EventArgs e)
        {
          
               
            LocaldriverLicenseInfo =
            clsDriverLicenseInfo.GetActiveDriverLicenseWithClass3Info(Convert.ToInt32(txtFilter.Text));
            if (LocaldriverLicenseInfo != null)
            {
                driverLicenseInfoCard1.Fill(LocaldriverLicenseInfo.LicenseID);
                  ILID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID
                    (driverLicenseInfoCard1.DriverLicenseInfo.DriverID);
                _CheckForSystemError();
                if(ILID != -1)
                {
                    MessageBox.Show($"Person Already has an active international License With ID = {ILID}"
                  , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError($"Person Already has an active international License With ID = {ILID}", EventLogEntryType.Error);
                    btnIssue.Enabled = false;
                    llblLShowLicensesInfo.Enabled = true;
                }
                else
                {
                    btnIssue.Enabled = true;
                    llblLShowLicensesInfo.Enabled = false;
                }
                MakeChangesAfterDriverCardFilled();
            }
            else
            {
                MessageBox.Show($"there is not Active Ordinary Driving License with License ID = {txtFilter.Text}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError($"there is not Active Ordinary Driving License with License ID = {txtFilter.Text}"
                    , EventLogEntryType.Error);
            }
        }

        private void NewInternationalLicense_Load(object sender, EventArgs e)
        {
            string Datestring = DateTime.Now.ToString("dd/MMM/yyyy");
            lblApplicationDate.Text = Datestring;
            lblIssueDate.Text = Datestring;
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("dd/MMM/yyyy");
            lblFees.Text = clsApplicationType.GetApplicationTypeFees(
                clsApplication.enApplicationType.NewInternationalLicense).ToString();
            lblUser.Text = clsGlobal.CurrentUser.UserName;
        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;

            if (textBox.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "Don't leave License ID blank");
            }
            else if (Regex.IsMatch(textBox.Text, @"^[0-9]+$") )
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "please enter a numeric value [0-9]");
            }
        }
        clsApplication Application = new clsApplication();
        private bool _AddNewApplication()
        {
            Application.ApplicantPersonID = driverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = 6;
            Application.ApplicationStatus = clsGlobal.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = Convert.ToDouble(lblFees.Text);
            Application.UserID = clsGlobal.CurrentUserID;
            if (Application.Save())
            {
                lblApplicationID.Text = Application.ApplicationID.ToString();
                return true;
            }
            else
                return false;
        }
        private void _AddNewInternationalLicense()
        {
            clsInternationalLicense IL = new clsInternationalLicense();
            IL.LLID =Convert.ToInt32( lblLLID.Text);
            IL.ApplicationID = Application.ApplicationID;
            IL.ExpirationDate = DateTime.Now.AddYears(1);
            IL.IssueDate = DateTime.Now;
            IL.DriverID = LocaldriverLicenseInfo.DriverID;
            IL.UserID = clsGlobal.CurrentUserID;
            if (IL.Save())
            {
                MessageBox.Show("International License Issued Successfully with ID=" +
                    IL.ILID.ToString(), "License Issued",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                clsValidations.LogTheError("International License Issued Successfully with ID=" +
                    IL.ILID.ToString(), EventLogEntryType.Error);
                ILID = IL.ILID;
                lblILID.Text = IL.ILID.ToString();
                llblLShowLicensesInfo.Enabled = true;
                gbxFilter.Enabled = false;
                btnIssue.Enabled = false;
              
            }
            else
            _ShowSystemError();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You sure you want to Issue The License?","Confirm",MessageBoxButtons.YesNo
                ,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!_AddNewApplication()) return;
                _AddNewInternationalLicense();
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LicenseHistory lh = new LicenseHistory(driverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID);
            lh.ShowDialog();
        }

        private void llblLShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          frmDriverInternationalLicenseInfo internationalLicenseInfo =
                new frmDriverInternationalLicenseInfo(ILID,true);
            internationalLicenseInfo.ShowDialog();
        }
    }
}
