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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Forms
{
    public partial class LocalDrivingLicenseApplications : Form
    {
        public LocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }
        private DataTable _dt = new DataTable();
        private void btnclose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _FillTheGridView()
        {
            dgvListLDLApplications.DataSource = _dt;
            if(_dt.Rows.Count > 0 )
            {

                dgvListLDLApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvListLDLApplications.Columns[0].Width = 120;

                dgvListLDLApplications.Columns[1].HeaderText = "Driving Class";
                dgvListLDLApplications.Columns[1].Width = 300;

                dgvListLDLApplications.Columns[2].HeaderText = "National No.";
                dgvListLDLApplications.Columns[2].Width = 150;

                dgvListLDLApplications.Columns[3].HeaderText = "Full Name";
                dgvListLDLApplications.Columns[3].Width = 350;

                dgvListLDLApplications.Columns[4].HeaderText = "Application Date";
                dgvListLDLApplications.Columns[4].Width = 170;

                dgvListLDLApplications.Columns[5].HeaderText = "Passed Tests";
                dgvListLDLApplications.Columns[5].Width = 80;
            }
        }
        private void _RefreshLDLApplicationsList()
        {
            _dt = clsLocalDrivingLicenseApplication.ListLocalDrivingLicenseApplications();
            _FillTheGridView();
            lblRecords.Text = dgvListLDLApplications.Rows.Count.ToString();
        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);
            this.Close();
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                clsGlobal.IsSystemError = false;
                this.Close();
            }
            else if (!clsGlobal.IsValidID)
            {
                MessageBox.Show("Invalid ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsGlobal.IsValidID = true;
            }
        }
        private void LocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _RefreshLDLApplicationsList();
            cbxFilter.SelectedIndex = 0;
            _CheckForSystemError();
         
        }

        private void cbxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (cbxStatus.Text == "All")
                _dt.DefaultView.RowFilter = "";
            else
                _dt.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "Status", cbxStatus.Text);
            lblRecords.Text = _dt.Rows.Count.ToString();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            if(_dt.Rows.Count > 0)
            {
            switch (cbxFilter.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "NationalNo.":
                    FilterColumn = "NationalNo";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;
                case "Full Name":
                        FilterColumn = "FullName";
                        break;
            }
            if (txtFilter.Text.Trim() == "" )
                _dt.DefaultView.RowFilter = "";
            else if (FilterColumn == "LocalDrivingLicenseApplicationID")
                _dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else if(FilterColumn == "FullName"||FilterColumn == "NationalNo")
                _dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvListLDLApplications.Rows.Count.ToString();

            }
        }

        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtFilter.Visible = (cbxFilter.Text != "None" && cbxFilter.Text != "Status");
            cbxStatus.Visible = (cbxFilter.Text == "Status");
            if(txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
            if (cbxStatus.Visible)
            {
              cbxStatus.SelectedIndex = 0;  
            }
            _dt.DefaultView.RowFilter = "";
            lblRecords.Text = dgvListLDLApplications.Rows.Count.ToString();
        }

        private void btnAddNewLocalDrivingLicenseApplication_Click(object sender, EventArgs e)
        {

            NewOrUpdateLocalDrivingLicense frmAddNewLocalDrivingLicense = new NewOrUpdateLocalDrivingLicense();
            frmAddNewLocalDrivingLicense.ShowDialog();
            _RefreshLDLApplicationsList();
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments visionTestAppointments = new frmListTestAppointments((int)dgvListLDLApplications.CurrentRow.Cells[0].Value,clsTestTypes.enTestType.VisionTest);
            visionTestAppointments.ShowDialog(); 
            _RefreshLDLApplicationsList();
        }

        private void scheduleTestsToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            scheduleVisionTestToolStripMenuItem.Enabled = !clsLocalDrivingLicenseApplication.IsPassedThisTestType(
                (int)dgvListLDLApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.VisionTest);
            if(scheduleVisionTestToolStripMenuItem.Enabled)return;
            scheduleWrittenTest.Enabled = !clsLocalDrivingLicenseApplication.IsPassedThisTestType(
               (int)dgvListLDLApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.WrittenTest);
            if (scheduleWrittenTest.Enabled) return;
            scheduleTestsToolStripMenuItem.Enabled = !clsLocalDrivingLicenseApplication.IsPassedThisTestType(
                (int)dgvListLDLApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.StreetTest);
            if (!scheduleTestsToolStripMenuItem.Enabled)
            {
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                return;
            }
            ScheduleStreetTest.Enabled = scheduleTestsToolStripMenuItem.Enabled;
        }

        private void scheduleWrittenTest_Click(object sender, EventArgs e)
        {
            frmListTestAppointments writtenTestAppointments = new frmListTestAppointments(
                (int)dgvListLDLApplications.CurrentRow.Cells[0].Value,clsTestTypes.enTestType.WrittenTest);
            writtenTestAppointments.ShowDialog();
            _RefreshLDLApplicationsList();
        }

        private void ScheduleStreetTest_Click(object sender, EventArgs e)
        {
            frmListTestAppointments streetTestAppointments = new frmListTestAppointments(
                (int)dgvListLDLApplications.CurrentRow.Cells[0].Value,clsTestTypes.enTestType.StreetTest);
            streetTestAppointments.ShowDialog();
            _RefreshLDLApplicationsList();
        }

        private void _ActionsAfterStatusCompletedOrCancelled(clsGlobal.enApplicationStatus Status)
        {
            EditApplication.Enabled = false;
            DeleteApplication.Enabled = false;
            CancelApplication.Enabled = false;
            scheduleTestsToolStripMenuItem.Enabled = false;
            if (Status == clsGlobal.enApplicationStatus.Completed)
                showLicenseToolStripMenuItem.Enabled = true;
            else showLicenseToolStripMenuItem.Enabled = false;
             issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
        }

        private void _ResetTheContextMenueStrip()
        {
            EditApplication.Enabled = true;
            DeleteApplication.Enabled = true;
            CancelApplication.Enabled = true;
            scheduleTestsToolStripMenuItem.Enabled= true;
            scheduleVisionTestToolStripMenuItem.Enabled = false;
            scheduleWrittenTest.Enabled= false;
            ScheduleStreetTest.Enabled = false;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;

        }
        private void dgvListLDLApplications_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            _ResetTheContextMenueStrip();
            clsGlobal.enApplicationStatus Status =
                clsApplication.GetApplicationStatus((int)dgvListLDLApplications.CurrentRow.Cells[0].Value);
            _CheckForSystemError();
            if (Status == clsGlobal.enApplicationStatus.Completed || Status == clsGlobal.enApplicationStatus.Cancelled)
            {
                _ActionsAfterStatusCompletedOrCancelled(Status);
                return;
            }
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IssueDriverLicenseForTheFirstTime issueDriverLicenseForTheFirstTime = new IssueDriverLicenseForTheFirstTime(
                (int)dgvListLDLApplications.CurrentRow.Cells[0].Value);
            issueDriverLicenseForTheFirstTime.ShowDialog();
            _RefreshLDLApplicationsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = clsLocalDrivingLicenseApplication.GetLocalLicenseIDByLDLAppID
                ((int)dgvListLDLApplications.CurrentRow.Cells[0].Value);
            if(LicenseID == -1)_ShowSystemError();
            DriverLicenseInfo frmLicenseInfo = new DriverLicenseInfo(LicenseID);
            frmLicenseInfo.ShowDialog();
        }

        private void EditApplication_Click(object sender, EventArgs e)
        {
            NewOrUpdateLocalDrivingLicense UpdateLocalDrivingLicenseApplication = new NewOrUpdateLocalDrivingLicense((int)dgvListLDLApplications.CurrentRow.Cells[0].Value);
            UpdateLocalDrivingLicenseApplication.ShowDialog();
            _RefreshLDLApplicationsList();
        }

        private void DeleteApplication_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to Delete this Application?", "Confirm",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {
                int LApplicationID = (int)dgvListLDLApplications.CurrentRow.Cells[0].Value;
                int ApplicationID = clsLocalDrivingLicenseApplication.GetApplicationID(LApplicationID);
                clsLocalDrivingLicenseApplication LDLApp = clsLocalDrivingLicenseApplication.FindLDLApplicationByID(LApplicationID);
                clsApplication App = clsApplication.FindApplicationByID(ApplicationID);
                if (LDLApp != null || App == null)
                {
                    if (LDLApp.Delete() &&  App.Delete())
                    {
                        _RefreshLDLApplicationsList();
                        MessageBox.Show("Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clsValidations.LogTheError("Deleted Successfully", EventLogEntryType.Information);
                    }
                    else
                    {
                        MessageBox.Show("The Application Can't Be deleted it has related data", "Delete failed",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsValidations.LogTheError("The Application Can't Be deleted it has related data", EventLogEntryType.Error);

                    }
                }
                else
                {
                    MessageBox.Show("The Application Not Found,May be deleted!", "Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);
                }

            }
        }

        private void CancelApplication_Click(object sender, EventArgs e)
        {
            int LApplicationID = (int)dgvListLDLApplications.CurrentRow.Cells[0].Value;
            int ApplicationID = clsLocalDrivingLicenseApplication.GetApplicationID(LApplicationID);
            clsApplication App = clsApplication.FindApplicationByID(ApplicationID);
            if ( App != null)
            {
                if (App.Cancel())
                {
                    _RefreshLDLApplicationsList();
                    MessageBox.Show("Cancelled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsValidations.LogTheError("Application Cancelled Successfully", EventLogEntryType.Error);

                }
                else
                {
                    MessageBox.Show("The Application Can't Be Cancelled", "Cancelled failed",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("The Application Can't Be Cancelled", EventLogEntryType.Error);
                }
            }
            else
            {
                MessageBox.Show("The Application Not Found,May be deleted!", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("The Application Not Found,May be deleted!", EventLogEntryType.Error);

            }

        }

        private void ShowApplicationDetails_Click(object sender, EventArgs e)
        {
            ApplicationDetails frmApplicationDetails = new ApplicationDetails
                ((int)dgvListLDLApplications.CurrentRow.Cells[0].Value);
            frmApplicationDetails.ShowDialog();
            _RefreshLDLApplicationsList();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsLocalDrivingLicenseApplication.GetPersonIDByLDLAppID(
                (int)dgvListLDLApplications.CurrentRow.Cells[0].Value);
            _CheckForSystemError();
            LicenseHistory licenseHistory = new LicenseHistory(PersonID);
            licenseHistory.ShowDialog();
            _RefreshLDLApplicationsList();
        }
    }
}
