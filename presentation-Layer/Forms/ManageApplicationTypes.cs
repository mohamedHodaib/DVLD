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
using DVLD_BussinessLayer;
namespace DVLD.Forms
{
    public partial class ManageApplicationTypes : Form
    {
        public ManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         private DataTable _dt = new DataTable();

        private void _FillTheGridView ()
        {
            dgvApplicationTypes.DataSource = _dt;
            dgvApplicationTypes.Columns[0].HeaderText = "User ID";
            dgvApplicationTypes.Columns[0].Width = 110;
            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 120;
            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 250;
        }
        private void _RefreshApplicationTypesList()
        {
            _dt = clsApplicationType.GetApplicationTypes();
            
            if(_dt.Rows.Count == 0)
            {
                MessageBox.Show("The is a system error We will solve Try Again Now or Later,The Window will " +
                    "Close!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                clsValidations.LogTheError("The is a system error We will solve Try Again Now or Later", EventLogEntryType.Error);

                this.Close();
            }
            _FillTheGridView();
        }
        private void ManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationTypesList();
            lblRecords.Text = _dt.Rows.Count.ToString();
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateApplicationType frmUpdateApplicationType = new UpdateApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
           frmUpdateApplicationType.ShowDialog();
            _RefreshApplicationTypesList();
        }
    }
}
