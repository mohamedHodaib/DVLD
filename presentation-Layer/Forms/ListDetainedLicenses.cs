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
    public partial class ListDetainedLicenses : Form
    {
        public ListDetainedLicenses()
        {
            InitializeComponent();
        }

        private DataTable _dt = new DataTable();
        private void ListDetainedLicenses_Load(object sender, EventArgs e)
        {
            cbxFilter.SelectedIndex = 0;
            _RefreshDetainedLicenseList();
            _CheckForSystemError();
            lblRecords.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicense releaseDetainedLicense = new ReleaseDetainedLicense();
            releaseDetainedLicense.ShowDialog();
            _RefreshDetainedLicenseList();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            Detain_License detainLicense = new Detain_License();
            detainLicense.ShowDialog();
            _RefreshDetainedLicenseList();
        }
        private void _RefreshDetainedLicenseList()
        {
            _dt = clsDetainedLicense.ListDetainedLicenses();
            _FillTheGridView();
        }
        private void _FillTheGridView()
        {
            dgvListDetainedLicenses.DataSource = _dt;
            JustifyColumns();
        }
        private void JustifyColumns()
        {
            if(dgvListDetainedLicenses.Rows.Count != 0)
            {
            dgvListDetainedLicenses.Columns[2].Width = 180;
            dgvListDetainedLicenses.Columns[5].Width = 180;
            dgvListDetainedLicenses.Columns[6].Width = 150;
            dgvListDetainedLicenses.Columns[7].Width = 200;
            }
        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);
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

            _dt = clsDetainedLicense.Filter(cbxFilter.Text, FilterText);
            _CheckForSystemError();
            _FillTheGridView();
        }
        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterText = "";
            if (cbxFilter.SelectedIndex == 0)
            {
                txtFilter.Visible = false;
                cbxIsReleased.Visible = false;
            }
            else if (cbxFilter.SelectedIndex == 2)
            {
                txtFilter.Visible = false;
                cbxIsReleased.Visible = true;
                cbxIsReleased.SelectedIndex = 0;
                FilterText = cbxIsReleased.Text;
            }
            else
            {
                cbxIsReleased.Visible = false;
                txtFilter.Visible = true;
                FilterText = txtFilter.Text;
            }
            FilterTheDataInTheGridView(FilterText);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView(txtFilter.Text);
        }

        private void cbxIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView(cbxIsReleased.SelectedItem.ToString());
        }

        private void btnclose1_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = clsLocalLicense.GetDriverID((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            int PersonID = clsDriver.GetPersonID(DriverID);
            ShowPersonDetails personDetails = 
                new ShowPersonDetails(PersonID);
            personDetails.ShowDialog();
            _RefreshDetainedLicenseList();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DriverLicenseInfo driverLicenseInfo = new DriverLicenseInfo((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            driverLicenseInfo.ShowDialog();
            
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = clsLocalLicense.GetDriverID((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            int PersonID =
                clsDriver.GetPersonID(DriverID);
          LicenseHistory LH = new LicenseHistory(PersonID);
            LH.ShowDialog();
            _RefreshDetainedLicenseList();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicense releaseDetainedLicense = 
                new ReleaseDetainedLicense((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            releaseDetainedLicense.ShowDialog();
            _RefreshDetainedLicenseList();
        }

        private void dgvListDetainedLicenses_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled =
                !(bool)dgvListDetainedLicenses.CurrentRow.Cells[3].Value;
        }
    }
}
