using DVLD.MyControls;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Forms
{
    public partial class ReplacementForDamagedorLost : Form
    {
        public ReplacementForDamagedorLost()
        {
            InitializeComponent();
        }
        clsDriverLicenseInfo OldLocaldriverLicenseInfo = new clsDriverLicenseInfo();
        clsApplication Application = new clsApplication();
        clsLocalLicense NewLLicense = new clsLocalLicense();
        clsLocalDrivingLicenseApplication LApplication = new clsLocalDrivingLicenseApplication();
        enum enReplaceType { RForDamaged = 4, RForLost = 3 };
        enReplaceType ReplaceType = enReplaceType.RForDamaged;

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            llblShowLicensesHistory.Enabled = (OldLocaldriverLicenseInfo.LicenseID != -1);
            lblOLID.Text = OldLocaldriverLicenseInfo.LicenseID.ToString();
        }
        private void btnGetLicenseInfo_Click(object sender, EventArgs e)
        {
            OldLocaldriverLicenseInfo =
          clsDriverLicenseInfo.GetDriverLicenseInfoWithLicenseID(Convert.ToInt32(txtFilter.Text));
            _CheckForSystemError();
            if (OldLocaldriverLicenseInfo != null)
            {
                driverLicenseInfoCard1.Fill(OldLocaldriverLicenseInfo.LicenseID);
                MakeChangesAfterDriverCardFilled();
                if (!OldLocaldriverLicenseInfo.IsActive)
                    MessageBox.Show($"Selected License is Not Active!,Choose another one"
                   , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    _CheckForSystemError();
                    btnIssue.Enabled = true;
                }
            }
            else
                MessageBox.Show($"there is no Driving License with License ID = {txtFilter.Text}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ReplacementForDamagedorLost_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblUser.Text = clsGlobal.CurrentUser.UserName;
            rbDamagedLicense.Checked = true;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            if(rbDamagedLicense.Checked)
            {
                lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(
                    clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ToString();
                lblTitle.Text = "Replacement For Damaged";
                this.Text = lblTitle.Text;
                ReplaceType = enReplaceType.RForDamaged;
            }
            else
            {
                lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(
                    clsApplication.enApplicationType.ReplaceLostDrivingLicense).ToString();
                lblTitle.Text = "Replacement For Lost";
                this.Text = lblTitle.Text;
                ReplaceType = enReplaceType.RForLost;
            }
        }
        public void MakeChangesAfterNewLicenseAdded()
        {
            lblRLID.Text = NewLLicense.LicenseID.ToString();
            llblLShowLicensesInfo.Enabled = true;
            gbxFilter.Enabled = false;
            gbxRFor.Enabled = false;
            btnIssue.Enabled = false;
        }
        private bool _AddNewLocalLicense()
        {
            NewLLicense.ApplicationID = Application.ApplicationID;
            NewLLicense.IssueDate = OldLocaldriverLicenseInfo.IssueDate;
            NewLLicense.ExpirationDate = OldLocaldriverLicenseInfo.ExpirationDate;
            NewLLicense.DriverID = OldLocaldriverLicenseInfo.DriverID;
            NewLLicense.IssueReason = Convert.ToByte(ReplaceType);
            NewLLicense.PaidFees = 0;
            NewLLicense.LicenseClass = clsDrivingLicenseClass.FindClassIDByClassName
                (clsDrivingLicenseClass.FindClassNameByClassID(OldLocaldriverLicenseInfo.LicenseClass));
            NewLLicense.Notes = "";
            NewLLicense.UserID = clsGlobal.CurrentUserID;
            if (NewLLicense.Save())
            {
                MakeChangesAfterNewLicenseAdded();
                MessageBox.Show($"License Replaced Successfully with ID = {NewLLicense.UserID}","Replace succeed"
                    ,MessageBoxButtons.OK,MessageBoxIcon.Information);
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
        private bool _Replace()
        {
            return _AddNewLocalLicense();
        }
        private bool _AddNewApplication()
        {
            Application.ApplicantPersonID = driverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = Convert.ToInt32(ReplaceType);
            Application.ApplicationStatus = clsGlobal.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = Convert.ToDouble(lblApplicationFees.Text);
            Application.UserID = clsGlobal.CurrentUserID;
            if (Application.Save())
            {
                lblApplicationID.Text = Application.ApplicationID.ToString();
                return true;    
            }
            else
               return false;
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want to Replacement The License?", "Confirm", MessageBoxButtons.YesNo
            , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!_AddNewApplication()) return;
               if(! _Replace()) return;
                _DeActiveOldLicense();
            }
        }

        private void llblLShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInfo LocalDriverLicenseInfo =
            new DriverLicenseInfo(NewLLicense.LicenseID);
            LocalDriverLicenseInfo.ShowDialog();
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LicenseHistory LH = new LicenseHistory(driverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID);
            LH.ShowDialog();
        }
    }
}
