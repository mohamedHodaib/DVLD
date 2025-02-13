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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Forms
{
    public partial class Detain_License : Form
    {
        public Detain_License()
        {
            InitializeComponent();
        }
        clsDriverLicenseInfo driverLicenseInfo = new clsDriverLicenseInfo();
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Detain_License_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblUser.Text = clsGlobal.CurrentUser.UserName;
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
        private void btnGetLicenseInfo_Click(object sender, EventArgs e)
        {
            driverLicenseInfo =
          clsDriverLicenseInfo.GetDriverLicenseInfoWithLicenseID(Convert.ToInt32(txtFilter.Text));
            _CheckForSystemError();
            if (driverLicenseInfo != null)
            {
                driverLicenseInfoCard1.Fill(driverLicenseInfo.LicenseID);
                MakeChangesAfterDriverCardFilled();
                if (driverLicenseInfo.IsLicenseExpired() || !driverLicenseInfo.IsActive)
                {
                    MessageBox.Show($"Selected License is Not Active or Expired "
                   , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Selected License is Not Active or Expired ", EventLogEntryType.Error);

                }
                else if (driverLicenseInfo.IsDetaind)
                {
                    MessageBox.Show("License already Detained, choose another one!"
              , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("License already Detained, choose another one!", EventLogEntryType.Error);
                }
                else
                {
                    _CheckForSystemError();
                    btnDetain.Enabled = true;
                    txtFineFees.Enabled = true;
                    txtFineFees.Focus();
                }
            }
            else
            {
                MessageBox.Show($"there is no Driving License with License ID = {txtFilter.Text}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError($"there is no Driving License with License ID = {txtFilter.Text}", EventLogEntryType.Error);

            }
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;

            if (textBox.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "Don't leave Fine Fees field blank");
            }
            else if (double.TryParse(textBox.Text, out double Fees))
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "please enter a numeric value!");
            }
        }

        private void _MakeChangesAfterDetainLicense()
        {
            llblLShowLicensesInfo.Enabled = true;
            gbxFilter.Enabled = false;
            lblDetainID.Enabled = false;
            btnDetain.Enabled = false;
            txtFineFees.Enabled = false;

        }
        private void _DetainLicense()
        {
            if (driverLicenseInfo.Detain(Convert.ToDouble(txtFineFees.Text)))
            {
                _MakeChangesAfterDetainLicense();
                MessageBox.Show($"License Detained Successfully with ID = " +
                    $"{driverLicenseInfo.DetainedLicense.DetainID}"
                   , "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clsValidations.LogTheError($"License Detained Successfully with ID ={driverLicenseInfo.DetainedLicense.DetainID}", EventLogEntryType.Information);
            }
            else
                _ShowSystemError();
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want to Detain The License?", "Confirm", MessageBoxButtons.YesNo
            , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _DetainLicense();
            }
        }

        private void llblLShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInfo LocalDriverLicenseInfo =
             new DriverLicenseInfo(driverLicenseInfo.LicenseID);
            LocalDriverLicenseInfo.ShowDialog();
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LicenseHistory LH = new 
                LicenseHistory
                (driverLicenseInfoCard1.DriverLicenseInfo.DriverInfo.PersonID);
            LH.ShowDialog();
        }
    }
}
