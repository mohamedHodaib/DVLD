using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Forms
{
    public partial class ReleaseDetainedLicense : Form
    {
        public ReleaseDetainedLicense()
        {
            InitializeComponent();
        }
        public ReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            txtFilter.Text = LicenseID.ToString();
            gbxFilter.Enabled = false;
            btnGetLicenseInfo_Click(null, null);
        }
        clsDriverLicenseInfo driverLicenseInfo = new clsDriverLicenseInfo();
        clsApplication Application = new clsApplication();
        clsLocalDrivingLicenseApplication LApplication = new clsLocalDrivingLicenseApplication();
        clsGlobal.stDetainedLicenseInfo DetainedLicense = new clsGlobal.stDetainedLicenseInfo();
        private void ReleaseDetainedLicense_Load(object sender, EventArgs e)
        {

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
        private void MakeChangesAfterDriverCardFilled()
        {
            lblLID.Text = driverLicenseInfo.LicenseID.ToString();
            llblShowLicensesHistory.Enabled = true;
        }
        private void MakeChangesAfterValidationIsTrue()
        {
            btnRelease.Enabled = true;
             DetainedLicense =
                clsDetainedLicense.GetDetainedLicense(driverLicenseInfo.LicenseID);
            if (DetainedLicense.DetainedID == 0) _ShowSystemError();
            lblDetainID.Text = DetainedLicense.DetainedID.ToString();
            lblDetainDate.Text = DetainedLicense.DetainDate.ToString();
            lblFineFees.Text = DetainedLicense.FineFees.ToString();
            lblUser.Text = DetainedLicense.CreatedByUserID.ToString();
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ToString();
            lblTotalFees.Text = 
                (Convert.ToDouble(lblApplicationFees.Text) + Convert.ToDouble(lblFineFees.Text)).ToString();
        }

        private void btnGetLicenseInfo_Click(object sender, EventArgs e)
        {
            driverLicenseInfo =
         clsDriverLicenseInfo.GetDriverLicenseInfoWithLicenseID(Convert.ToInt32(txtFilter.Text));
            _CheckForSystemError();
            if (driverLicenseInfo != null)
            {
                ctrDriverLicenseInfoCard1.Fill(driverLicenseInfo.LicenseID);
                MakeChangesAfterDriverCardFilled();
                if(!driverLicenseInfo.IsActive || clsLocalLicense.IsExpired(driverLicenseInfo.LicenseID))
                    {
                        MessageBox.Show($"Selected License is Not Active or Expired"
                      , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if(!driverLicenseInfo.IsDetaind)
                    MessageBox.Show("License Is not Detained, choose another one!"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    _CheckForSystemError();
                    MakeChangesAfterValidationIsTrue();
                }
            }
            else
                MessageBox.Show($"there is no Driving License with License ID = {txtFilter.Text}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LicenseHistory LH = new LicenseHistory(ctrDriverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID);
            LH.ShowDialog();
        }

        private void llblLShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInfo LocalDriverLicenseInfo =
             new DriverLicenseInfo(driverLicenseInfo.LicenseID);
            LocalDriverLicenseInfo.ShowDialog();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _AddNewApplication()
        {
            Application.ApplicantPersonID = ctrDriverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = 5;
            Application.ApplicationStatus = clsGlobal.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = Convert.ToDouble(lblTotalFees.Text);
            Application.UserID = clsGlobal.CurrentUserID;
            if (Application.Save())
            {
                lblApplicationID.Text = Application.ApplicationID.ToString();
            }
            else
                _ShowSystemError();
        }

        private bool _Release()
        {
            return clsDetainedLicense.Release(DetainedLicense.DetainedID,
                 clsGlobal.CurrentUserID, Application.ApplicationID);
        }
        private void MakeChangesAfterRelease()
        {
            btnRelease.Enabled = false;
            gbxFilter.Enabled = false;
            llblLShowLicensesInfo.Enabled = true;
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want to Release This License?", "Confirm", MessageBoxButtons.YesNo
             , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _AddNewApplication();
                if (_Release())
                {
                    MessageBox.Show("License Released Successfully",
                        "Success", MessageBoxButtons.OK
                     , MessageBoxIcon.Information);
                    MakeChangesAfterRelease();
                }
                else
                    _ShowSystemError();
            }
        }
    }
}
