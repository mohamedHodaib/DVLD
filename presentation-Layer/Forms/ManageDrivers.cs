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
    public partial class ManageDrivers : Form
    {
        public ManageDrivers()
        {
            InitializeComponent();
         
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataTable _dtDriversList = new DataTable();
        private void _FillTheGridView()
        {
            dgvDrivers.DataSource = _dtDriversList;
            _JustifyTheGridView();
        }

        private void _RefreshDriversList()
        {
            _dtDriversList = clsDriver.ListDrivers();
            _FillTheGridView();
            lblRecords.Text = dgvDrivers.Rows.Count.ToString();
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
                _ShowSystemError();
                clsGlobal.IsSystemError = false;
                this.Close();
            }
        }
        private void _JustifyTheGridView()
        {
            if (_dtDriversList.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].Width = 80;
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[1].Width = 100;
                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[2].Width = 250;
                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[3].Width = 250;
                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[4].Width = 200;
                dgvDrivers.Columns[4].HeaderText = "Created Date";
                dgvDrivers.Columns[5].Width = 170;
                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
            }
        }
        private void ManageDrivers_Load(object sender, EventArgs e)
        {
            cbxFilter.SelectedIndex = 0;
            _RefreshDriversList();
            _CheckForSystemError();
        }
        private void FilterTheDataInTheGridView()
        {

            string FilterColumn = "";
            switch (cbxFilter.Text)
            {
                case "User ID":
                    FilterColumn = "DriverID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No":
                    FilterColumn = "NationalNumber";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
                _dtDriversList.DefaultView.RowFilter = "";
            else if (FilterColumn == "PersonID" || FilterColumn == "DriverID")
                _dtDriversList.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtDriversList.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = _dtDriversList.Rows.Count.ToString();
        }
        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxFilter.SelectedIndex == 0)
            {
                txtFilter.Visible = false;
                FilterTheDataInTheGridView() ;
            }
            else
            {
                txtFilter.Visible = true;
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbxFilter.Text == "Person ID" ||cbxFilter.Text == "Driver ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); ;
        }


        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonDetails frmshowPersonDetails = new ShowPersonDetails((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frmshowPersonDetails.ShowDialog();
            _RefreshDriversList();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseHistory licenseHistory = new LicenseHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);
            licenseHistory.ShowDialog();
            _RefreshDriversList();
        }
    }
}
