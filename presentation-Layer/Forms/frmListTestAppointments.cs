using DVLD.Properties;
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
using static DVLD_BussinessLayer.clsTestTypes;

namespace DVLD.Forms
{
    public partial class frmListTestAppointments : Form
    {
       
        DataTable _dt = new DataTable();
        clsTestTypes.enTestType _TestType;
        int _LDLAppID = 0;
        public frmListTestAppointments(int LDLAppID,clsTestTypes.enTestType TestType)
        {
            InitializeComponent();
            _TestType = TestType;
            _LDLAppID = LDLAppID;
           clsGlobal.LDLApp = clsLocalDrivingLicenseApplication.FindLDLApplicationByID(LDLAppID);
            if( clsGlobal.LDLApp != null )
            {
            clsGlobal.App = clsApplication.FindApplicationByID(clsGlobal.LDLApp.ApplicationID);
                if (clsGlobal.App != null)
                {
                   ctrApplicationInfoCard1. FillTheApplicationFormWithData(clsGlobal.LDLApp,clsGlobal.App);
                    _GetAppointments();
                }
                else
                    _ShowSystemError();
            }
            else 
                _ShowSystemError();
        }
        private void _FillTheGridView()
        {
            dgvAppointments.DataSource = _dt;
            if (dgvAppointments.Rows.Count > 0)
            {
                dgvAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvAppointments.Columns[0].Width = 150;

                dgvAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvAppointments.Columns[1].Width = 200;

                dgvAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvAppointments.Columns[2].Width = 150;

                dgvAppointments.Columns[3].HeaderText = "Is Locked";
                dgvAppointments.Columns[3].Width = 100;
            }
        }
        private void _RefreshTheGridView()
        {
            _dt = clsAppointments.GetAppointments(clsGlobal.LDLApp.LocalDrivingLicenseApplicationID,(int)_TestType);

            if (_dt.Rows.Count == 0&&clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                this.Close();
            }
            _FillTheGridView();
            lblRecords.Text = _dt.Rows.Count.ToString();
        }

        private void _GetAppointments()
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

        private void btnAddAppointments_Click(object sender, EventArgs e)
        {
            if(clsAppointments.IsHaveAnActiveAppointment(_LDLAppID,_TestType))
            {
                MessageBox.Show("Person has an active Appointment for this test ," +
                    "you cannot add a new appointment","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Person has an active Appointment for this test", EventLogEntryType.Error);
            }
            else if(clsLocalDrivingLicenseApplication.IsPassedThisTestType(_LDLAppID,_TestType))
            {
                MessageBox.Show("Person has already passed this test ," +
                  "you can only retake Failed Test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Person has already passed this test ," +
                  "you can only retake Failed Test", EventLogEntryType.Error);
            }
            else if (clsGlobal.IsSystemError)
            {
                _ShowSystemError();
            }
            else
            {
                frmScheduleTest frm = new frmScheduleTest(_LDLAppID,_TestType);
                frm.ShowDialog();
                _RefreshTheGridView();
            }
            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleTest frmScheduleVisionTest = new frmScheduleTest(_LDLAppID, _TestType,
                (int)dgvAppointments
                .CurrentRow.Cells[0].Value);
            frmScheduleVisionTest.ShowDialog();
            _RefreshTheGridView();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TakeTest frmTakeTest = new TakeTest((int)dgvAppointments.CurrentRow.Cells[0].Value,_TestType);
            frmTakeTest.ShowDialog();
            _RefreshTheGridView();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            switch (_TestType)
            {

                case clsTestTypes.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestTypes.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestTypes.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Street_Test_322;
                        break;
                    }
            }
        }
    }
}
