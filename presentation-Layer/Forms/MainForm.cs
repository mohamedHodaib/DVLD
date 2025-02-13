using DVLD.Forms;
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

namespace DVLD
{
    public partial class MainForm : Form
    {
        LoginScreen _frmLogin;
         
        public MainForm( LoginScreen frmLogin)
        {
            InitializeComponent();
            _frmLogin = frmLogin;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople ManagingPeople = new frmManagePeople();
            ManagingPeople.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUsers frmManageUsers = new ManageUsers();
            frmManageUsers.ShowDialog();
        }

        private void currrentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserDetails frmUserDetails = new UserDetails(clsGlobal.CurrentUserID);
            frmUserDetails.ShowDialog();
        }

        private void changToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword frmChangePassword = new ChangePassword(clsGlobal.CurrentUserID);
            frmChangePassword.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Hide();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageApplicationTypes frmManageApplicationTypes = new ManageApplicationTypes();
            frmManageApplicationTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageTestTypes frmManageTestTypes = new ManageTestTypes();
            frmManageTestTypes.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewOrUpdateLocalDrivingLicense frmAddNewLocalDrivingLicense = new NewOrUpdateLocalDrivingLicense();
            frmAddNewLocalDrivingLicense.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplications frmlocalDrivingLicenseApplications = new LocalDrivingLicenseApplications();
            frmlocalDrivingLicenseApplications.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDrivers manageDrivers = new ManageDrivers();
            manageDrivers.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewInternationalLicense newInternationalLicense = new NewInternationalLicense();
            newInternationalLicense.ShowDialog();
        }

        private void internationalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternationalLicensesApplications internationalLicensesApplications = 
                new InternationalLicensesApplications();
            internationalLicensesApplications.ShowDialog();
        }

        private void RenewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewLocalDrivingLicense renewLocalDrivingLicense = 
                new RenewLocalDrivingLicense();
            renewLocalDrivingLicense.ShowDialog();
        }

        private void replacementForDamagedOrLostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacementForDamagedorLost replacementForDamagedorLost = new ReplacementForDamagedorLost();
            replacementForDamagedorLost.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Detain_License detain_License = new Detain_License();
            detain_License.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicense releaseLicense = new ReleaseDetainedLicense();
            releaseLicense.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           ListDetainedLicenses listDetainedLicenses = new ListDetainedLicenses();
            listDetainedLicenses.ShowDialog();
            
        }

        private void releaseDetainedLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           ReleaseDetainedLicense releaseDetainedLicense = new ReleaseDetainedLicense();
            releaseDetainedLicense.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplications localDrivingLicenseApplications
                = new LocalDrivingLicenseApplications();
            localDrivingLicenseApplications.ShowDialog();

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void drivingLiscencesServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
