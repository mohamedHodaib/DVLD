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
    public partial class LicenseHistory : Form
    {
        public LicenseHistory(int PersonID)
        {
            InitializeComponent();
            ctrPersonCard1.FillPersonDetails(PersonID);
            _FillLocalLicensesHistoryTab(PersonID);
            _FillInternationalLicensesHistoryTab(PersonID);
        }
        DataTable dt = new DataTable();
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                clsGlobal.IsSystemError = false;
                this.Close();
            }
        }
        private void _FillTheGridView(DataGridView dgv)
        {
           dgv.DataSource = dt;
        }

        private void JustifyTheGridView(DataGridView dgv)
        {
            if(dt.Rows.Count > 0)
            {
            dgv.Columns[0].Width = 100;
            dgv.Columns[1].Width = 100;
            dgv.Columns[2].Width = 250;
            dgv.Columns[3].Width = 250;
            dgv.Columns[4].Width = 250;
            dgv.Columns[5].Width = 100;

            }
        }
        private void _FillLocalLicensesHistoryTab(int PersonID)
        {
            dt = clsLocalLicense.GetLocalLicensesHistory(PersonID);
            _CheckForSystemError();
            _FillTheGridView(dgvLocal);
            JustifyTheGridView(dgvLocal);
            lblLocalRecords.Text = dt.Rows.Count.ToString();
        }

        private void _FillInternationalLicensesHistoryTab(int PersonID)
        {
            dt = clsInternationalLicense.GetInternationalLicensesHistory(PersonID);
            _CheckForSystemError();
            _FillTheGridView(dgvInternational);
            JustifyTheGridView(dgvInternational);
            lblInternationalRecords.Text = dt.Rows.Count.ToString();
        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);
            this.Close();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
