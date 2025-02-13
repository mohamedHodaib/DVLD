using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Forms
{
    public partial class RenewLocalDrivingLicense : Form
    {
        public RenewLocalDrivingLicense()
        {
            InitializeComponent();
        }
        clsDriverLicenseInfo OldLocaldriverLicenseInfo = new clsDriverLicenseInfo();
        clsApplication Application = new clsApplication();
        clsLocalLicense NewLLicense = new clsLocalLicense();
        clsLocalDrivingLicenseApplication LApplication = new clsLocalDrivingLicenseApplication();
        private void RenewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            txtFilter.Focus();
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(
                clsApplication.enApplicationType.RenewDrivingLicense).ToString();
            lblUser.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;

            if (textBox.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "Don't leave License ID blank");
            }
            else if (Regex.IsMatch(textBox.Text, @"^[0-9]+$"))
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
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void MakeChangesAfterDriverCardFilled()
        {
            llblShowLicensesHistory.Enabled = true;
            lblOldLID.Text = OldLocaldriverLicenseInfo.LicenseID.ToString();
            lblLicenseFees.Text = clsDrivingLicenseClass.GetFees(OldLocaldriverLicenseInfo.LicenseClass).ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(clsDrivingLicenseClass.GetValidityYears(
                OldLocaldriverLicenseInfo.LicenseClass)).ToString("dd/MMM/yyyy");
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            txtNotes.Text = OldLocaldriverLicenseInfo.Notes;
        }

        private void btnGetLicenseInfo_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            OldLocaldriverLicenseInfo =
          clsDriverLicenseInfo.GetDriverLicenseInfoWithLicenseID(Convert.ToInt32(txtFilter.Text));
            _CheckForSystemError();
            if (OldLocaldriverLicenseInfo != null)
            {
                ctrDriverLicenseInfoCard1.Fill(OldLocaldriverLicenseInfo.LicenseID);
                MakeChangesAfterDriverCardFilled();
                if (!OldLocaldriverLicenseInfo.IsActive)
                    MessageBox.Show($"Selected License is Not Active , Choose another one " 
                   , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (!OldLocaldriverLicenseInfo.IsLicenseExpired())
                    MessageBox.Show($"Selected License is Not Expired Yet,it will Expired  On " +
                      $"{OldLocaldriverLicenseInfo.ExpirationDate}"
                 , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if ( clsLocalLicense.IsDriverHasAnActiveLicenseInThisClass
                    (OldLocaldriverLicenseInfo.DriverID, OldLocaldriverLicenseInfo.LicenseClass,OldLocaldriverLicenseInfo.LicenseID) != -1)
                {
                    MessageBox.Show($"Person Has Already Active License with ID {LicenseID} " +
                       $"{OldLocaldriverLicenseInfo.ExpirationDate}"
                  , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    _CheckForSystemError();
                    btnRenew.Enabled = true;
                }
            }
            else
                MessageBox.Show($"there is no Driving License with License ID = {txtFilter.Text}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private bool _AddNewApplication()
        {
            Application.ApplicantPersonID = ctrDriverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = 2;
            Application.ApplicationStatus = clsGlobal.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = Convert.ToDouble(lblTotalFees.Text);
            Application.UserID = clsGlobal.CurrentUserID;
            if (Application.Save())
            {
                lblApplicationID.Text = Application.ApplicationID.ToString();
                return true;    
            }
            else
               return false;
        }
      
        public void MakeChangesAfterNewLicenseAdded()
        {
                lblRLID.Text = NewLLicense.LicenseID.ToString();
                llblLShowLicensesInfo.Enabled = true;
                gbxFilter.Enabled = false;
                btnRenew.Enabled = false;
        }
            
        private bool _AddNewLocalLicense()
        {
            NewLLicense.ApplicationID = Application.ApplicationID;
            NewLLicense.IssueDate = DateTime.Now;
            NewLLicense.ExpirationDate = DateTime.Now.AddYears(clsDrivingLicenseClass.
                GetValidityYears(OldLocaldriverLicenseInfo.LicenseClass));
            NewLLicense.DriverID = OldLocaldriverLicenseInfo.DriverID;
            NewLLicense.IssueReason = 2;
            NewLLicense.PaidFees = Convert.ToDouble(lblTotalFees.Text);
            NewLLicense.LicenseClass = OldLocaldriverLicenseInfo.LicenseClass;
            NewLLicense.Notes = txtNotes.Text;
            NewLLicense.UserID = clsGlobal.CurrentUserID;
            if (NewLLicense.Save())
            {
                MakeChangesAfterNewLicenseAdded();
                MessageBox.Show($"License Renewed Successfully with ID = {NewLLicense.UserID}", "Renew succeed"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
                MakeChangesAfterNewLicenseAdded();
                return true;
            }
            else
               return false;
        }
        private void _DeActiveOldLicense()
        {
            if (!clsLocalLicense.DeActive(OldLocaldriverLicenseInfo.LicenseID))
                _ShowSystemError();
        }
        private bool _Renew()
        {
            return _AddNewLocalLicense();
        }
        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want to Issue The License?", "Confirm", MessageBoxButtons.YesNo
              , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!_AddNewApplication()) return;
                if (!_Renew()) return;
                _DeActiveOldLicense();
            }
        }
        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LicenseHistory LH = new LicenseHistory(ctrDriverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID);
            LH.ShowDialog();
        }

        private void llblLShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInfo LocalDriverLicenseInfo =
              new DriverLicenseInfo(NewLLicense.LicenseID);
            LocalDriverLicenseInfo.ShowDialog();
        }
    }
}
