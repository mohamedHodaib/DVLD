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
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }
        private DataTable _dtUsers = new DataTable();
        private void _RefreshUsersList()
        {
            _dtUsers = clsUser.ListUsers();
            _FillTheGridView();
            lblRecords.Text = dgvListUsers.Rows.Count.ToString();
        }
        private void _FillTheGridView()
        {
            dgvListUsers.DataSource = _dtUsers;
            if(dgvListUsers.Rows.Count > 0)
            {
            dgvListUsers.Columns[0].HeaderText = "User ID";
            dgvListUsers.Columns[0].Width = 110;

            dgvListUsers.Columns[1].HeaderText = "Person ID";
            dgvListUsers.Columns[1].Width = 120;

            dgvListUsers.Columns[2].HeaderText = "Full Name";
            dgvListUsers.Columns[2].Width = 350;

            dgvListUsers.Columns[3].HeaderText = "User Name";
            dgvListUsers.Columns[3].Width = 120;

            dgvListUsers.Columns[4].HeaderText = "Is Active";
            dgvListUsers.Columns[4].Width = 120;
            }

        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There a system error we will solve try Again Later! ", EventLogEntryType.Error);
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                clsGlobal.IsSystemError = false;
            }
        }
        private void ManageUsers_Load(object sender, EventArgs e)
        {
            cbxFilter.SelectedIndex = 0;
            _RefreshUsersList();
            _CheckForSystemError();
            lblRecords.Text = dgvListUsers.Rows.Count.ToString();
        }

        private void btnclose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FilterTheDataInTheGridView()
        {

            string FilterColumn = "";
            switch (cbxFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
               
                case "User Name":
                    FilterColumn = "UserName";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
                _dtUsers.DefaultView.RowFilter = "";
            else if (FilterColumn == "PersonID" ||FilterColumn == "UserID")
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvListUsers.Rows.Count.ToString();
        }
        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxFilter.SelectedIndex == 0)
            {
                txtFilter.Visible = false;
                cbxIsActive.Visible = false;
              FilterTheDataInTheGridView();
            }
            else if (cbxFilter.SelectedIndex == 5)
            {
                txtFilter.Visible = false;
                cbxIsActive.Visible = true;
                cbxIsActive.SelectedIndex = 0;
            }
            else
            {
                
                cbxIsActive.Visible = false;
                txtFilter.Visible = true;
                txtFilter.Text = string.Empty;
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView();
        }

        private void cbxIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterValue = cbxIsActive.Text;
            switch(FilterValue)
            {
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if (FilterValue == "All")
                _dtUsers.DefaultView.RowFilter = "";
            else
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", FilterValue);
            lblRecords.Text = dgvListUsers.Rows.Count.ToString();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            AddOrUpdateUser frmaddOrUpdateUser = new AddOrUpdateUser();
            frmaddOrUpdateUser.ShowDialog();
            _RefreshUsersList();
        }

        private void showDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserDetails frmUserDetails = new UserDetails((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frmUserDetails.ShowDialog();
            _RefreshUsersList();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOrUpdateUser frmaddOrUpdateUser = new AddOrUpdateUser();
            frmaddOrUpdateUser.ShowDialog();
            _RefreshUsersList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOrUpdateUser frmAddOrUpdatePerson = new AddOrUpdateUser((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frmAddOrUpdatePerson.ShowDialog();
            _RefreshUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to Delete this User?", "Confirm",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {
                if (!clsUser.DeleteUser((int)dgvListUsers.CurrentRow.Cells[0].Value))
                {
                    if (clsGlobal.IsForeignKeyConstraintException)
                    {
                        MessageBox.Show("User has other related data,we can't delete", "Delete faild!"
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsGlobal.IsForeignKeyConstraintException = false;
                        clsValidations.LogTheError("User has other related data,we can't delete ", EventLogEntryType.Error);
                    }
                    else
                        _ShowSystemError();

                }
                else
                {
                    _RefreshUsersList();
                    MessageBox.Show("Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsValidations.LogTheError("Deleted Successfully!", EventLogEntryType.Information);
                }
            }
        }

        private void changePasswordMenueItem_Click(object sender, EventArgs e)
        {
            ChangePassword frmChangePassword = new ChangePassword((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frmChangePassword.ShowDialog();
            _RefreshUsersList();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbxFilter.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); ;
        }

        private void dgvListUsers_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UserDetails frmUserDetails = new UserDetails((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frmUserDetails.ShowDialog();
            _RefreshUsersList();
        }
    }
}
