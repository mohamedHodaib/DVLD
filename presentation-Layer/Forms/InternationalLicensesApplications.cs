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
    public partial class InternationalLicensesApplications : Form
    {
        DataTable _dt = new DataTable();
        public InternationalLicensesApplications()
        {
            InitializeComponent();
            _GetInternationalDrivingLicenseApplications();
        }
        private void _FillTheGridView()
        {
            dgvListIDLApplications.DataSource = _dt;
            if (dgvListIDLApplications.Rows.Count > 0)
            {
                    dgvListIDLApplications.Columns[1].Width = 200;
                dgvListIDLApplications.Columns[3].Width = 200;
                dgvListIDLApplications.Columns[4].Width = 150;
                dgvListIDLApplications.Columns[5].Width = 150;

            }

        }
        private void _RefreshTheGridView()
        {
            _dt = clsInternationalLicense.ListInternationalLicenses();
            if (_dt.Rows.Count == 0 && clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                this.Close();
            }
            _FillTheGridView();
            lblRecords.Text = _dt.Rows.Count.ToString();
        }

        private void _GetInternationalDrivingLicenseApplications()
        {
            _RefreshTheGridView();
        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);
            this.Close();
        }
        private void btnclose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                clsGlobal.IsSystemError = false;
            }
            else if (!clsGlobal.IsValidID)
            {
                MessageBox.Show("Invalid ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsGlobal.IsValidID = true;
            }
        }
        private void FilterTheDataInTheGridView(string FilterText)
        {

            _dt = clsInternationalLicense.Filter(cbxFilter.Text, FilterText);
            _CheckForSystemError();
            _FillTheGridView();
        }
        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterText = "";
            if (cbxFilter.SelectedIndex == 0)
            {
                txtFilter.Visible = false;
                cbxIsActive.Visible = false;
            }
            else if (cbxFilter.SelectedIndex == 5)
            {
                txtFilter.Visible = false;
                cbxIsActive.Visible = true;
                cbxIsActive.SelectedIndex = 0;
                FilterText = cbxIsActive.Text;
            }
            else
            {
                cbxIsActive.Visible = false;
                txtFilter.Visible = true;
                FilterText = txtFilter.Text;
            }
            FilterTheDataInTheGridView(FilterText);
        }

        private void cbxIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView(cbxIsActive.Text);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView(txtFilter.Text);
        }

        private void InternationalLicensesApplications_Load(object sender, EventArgs e)
        {
            cbxFilter.SelectedIndex = 0;
        }

        private void btnAddNewInternationalDrivingLicenseApplication_Click(object sender, EventArgs e)
        {
            NewInternationalLicense newInternationalLicense = new NewInternationalLicense();
            newInternationalLicense.ShowDialog();
        }

        private void ShowPersonDetails_Click(object sender, EventArgs e)
        {
            ShowPersonDetails showPersonDetails = new ShowPersonDetails(clsDriver.GetPersonID(
       (int)dgvListIDLApplications.CurrentRow.Cells[2].Value));
            showPersonDetails.ShowDialog();

        }

        private void ShowLicenseDetails_Click(object sender, EventArgs e)
        {
            frmDriverInternationalLicenseInfo frmDriverInternationalLicenseInfo = new frmDriverInternationalLicenseInfo(
             (int)dgvListIDLApplications.CurrentRow.Cells[0].Value,false);
            frmDriverInternationalLicenseInfo.ShowDialog();
        }

        private void ShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            LicenseHistory licenseHistory = new LicenseHistory(clsDriver.GetPersonID(
       (int)dgvListIDLApplications.CurrentRow.Cells[2].Value));
            licenseHistory.ShowDialog();
        }
    }
}
