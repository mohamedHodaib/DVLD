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
    public partial class ManageTestTypes : Form
    {
        public ManageTestTypes()
        {
            InitializeComponent();
        }

        private DataTable _dt = new DataTable();
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _FillTheGridView()
        {
            dgvTestTypes.DataSource = _dt;
            if(dgvTestTypes.Rows.Count > 0 )
            {
            dgvTestTypes.Columns[0].HeaderText = "ID";
            dgvTestTypes.Columns[0].Width = 120;

            dgvTestTypes.Columns[1].HeaderText = "Title";
            dgvTestTypes.Columns[1].Width = 150;

            dgvTestTypes.Columns[2].HeaderText = "Description";
            dgvTestTypes.Columns[2].Width = 400;

            dgvTestTypes.Columns[3].HeaderText = "Fees";
            dgvTestTypes.Columns[3].Width = 100;
            }
        }
        private void _RefreshTestTypesList()
        {
            _dt = clsTestTypes.GetTestTypes();

            if (_dt.Rows.Count == 0)
            {
                MessageBox.Show("The is a system error We will solve Try Again Now or Later,The Window will " +
                    "Close!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("There a system error we will solve try Again Later! ", EventLogEntryType.Error);
                this.Close();
            }
            _FillTheGridView();
        }
        private void ManageTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshTestTypesList();
            lblRecords.Text = _dt.Rows.Count.ToString();
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTestType frmUpdateTestType = new UpdateTestType((int)dgvTestTypes.CurrentRow.Cells[0].Value);
            frmUpdateTestType.ShowDialog();
            _RefreshTestTypesList();
        }
    }
}
