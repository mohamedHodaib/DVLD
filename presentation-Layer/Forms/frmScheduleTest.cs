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
    public partial class frmScheduleTest : Form
    {

        clsAppointments Appointment = new clsAppointments();
        private int _AppointmentID = -1, _LDLAppID = -1;
        clsTestTypes.enTestType _TestType = clsTestTypes.enTestType.VisionTest;
        private clsAppointments.enMode _Mode = clsAppointments.enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
        public frmScheduleTest(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestTypeID, int AppointmentID = -1)
        {
            InitializeComponent();
            _AppointmentID = AppointmentID;
            _LDLAppID = LocalDrivingLicenseApplicationID;
            _TestType = TestTypeID;
            _Mode = AppointmentID == -1?clsAppointments.enMode.AddNew: clsAppointments.enMode.Update;
            _FillTheFormWithData();
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
                _ShowSystemError();
        }

        private void _FillBasicInfo()
        {
            lblDLAppID.Text = _LDLAppID.ToString();
            lblDClass.Text = clsDrivingLicenseClass.FindClassNameByClassID(_LDLAppID);
            _CheckForSystemError();
            clsPerson P = clsPerson.FindByPersonID(clsLocalDrivingLicenseApplication.GetPersonIDByLDLAppID(_LDLAppID)).Result;
            _CheckForSystemError();
            lblName.Text = P.FName + " " + P.SName + " " + P.TName + " " + P.LName;
            lblTrial.Text = clsTests.GetTrials(_LDLAppID, _TestType).ToString();
            _CheckForSystemError();
            lblTestFees.Text = clsTestTypes.GetTestTypeFees(_TestType).ToString();
            lblTotalFees.Text = lblTestFees.Text;
            dtpAppointmentDate.MinDate = DateTime.Now;
            dtpAppointmentDate.Value = DateTime.Now;
        }
        private void _LoadAppointmentData()
        {
            Appointment = clsAppointments.Find(_AppointmentID);
            if (Appointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + Appointment.TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError(" No Appointment with ID = " + Appointment.TestAppointmentID.ToString(), EventLogEntryType.Error);
                btnSave.Enabled = false;
                return;
            }
            if (Appointment.IsLocked)
            {
                lblMessage.Visible = true;
                dtpAppointmentDate.Enabled = false;
                btnSave.Enabled = false;
            }
            else if (clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                this.Close();
            }
            _CheckForSystemError();
            dtpAppointmentDate.Value = Appointment.AppointmentDate;
            lblTestFees.Text = Appointment.PaidFees.ToString();
            if (Appointment.RetakeTestApplicationID == -1)
            {
                lblRAppFees.Text = "0";
                lblRTestAppID.Text = "N/A";
            }
            else
            {
                lblRAppFees.Text = Appointment.RetakeTestApplication.PaidFees.ToString();
                gbxRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRTestAppID.Text = Appointment.RetakeTestApplicationID.ToString();

            }
        }
        private void _FillTheFormWithData()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindLDLApplicationByID(_LDLAppID);
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LDLAppID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("No Local Driving License Application with ID = " + _LDLAppID.ToString(), EventLogEntryType.Error);
                btnSave.Enabled = false;
                return;
            }
            _FillBasicInfo();
            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestType))

                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;


            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRAppFees.Text = clsApplicationType.GetApplicationTypeFees(clsApplication.enApplicationType.RetakeTest).ToString();
                gbxRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRTestAppID.Text = "0";
            }
            if (_Mode == clsAppointments.enMode.Update)
            {
                _LoadAppointmentData();
            }
            else
            {
                Appointment = new clsAppointments();
                
            }

            lblTotalFees.Text = (Convert.ToDouble(lblTestFees.Text) + Convert.ToDouble(lblRAppFees.Text)).ToString();
        }

        private void scheduleVisionTest_Load(object sender, EventArgs e)
        {
            dtpAppointmentDate.MinDate = DateTime.Now;
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _FillTheApplicationObject( clsApplication App)
        {
            App.ApplicantPersonID = clsLocalDrivingLicenseApplication.GetPersonIDByLDLAppID(
                _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID);
            App.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
            App.ApplicationStatus = clsGlobal.enApplicationStatus.Completed;
            App.UserID = clsGlobal.CurrentUserID;
            App.ApplicationDate = DateTime.Now;
            App.LastStatusDate = DateTime.Now;
            App.PaidFees = clsApplicationType.GetApplicationTypeFees(clsApplication.enApplicationType.RetakeTest);
        }
        private bool _FillTheAppointmentObjectWithData()
        {
            Appointment.TestTypeID = _TestType;
            Appointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            Appointment.AppointmentDate = dtpAppointmentDate.Value;
            Appointment.PaidFees = Convert.ToDouble(lblTestFees.Text);
            Appointment.UserID = clsGlobal.CurrentUserID;
            if(_CreationMode == enCreationMode.RetakeTestSchedule && _Mode == clsAppointments.enMode.AddNew)
            {
              
                clsApplication _App = new clsApplication();
               _FillTheApplicationObject( _App ); 
               if(! _App.Save())
                {

                    MessageBox.Show("Saving Failed,something go wrong,may be a " +
                  "system error try again later!",
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Saving Failed", EventLogEntryType.Error);
                    return false;
                }

                
            Appointment.RetakeTestApplicationID = _App.ApplicationID;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
          if(  _FillTheAppointmentObjectWithData())
            {
            if(Appointment.Save())
            {
                    _Mode = clsAppointments.enMode.Update;
                    MessageBox.Show("Saved Successfully", "success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsValidations.LogTheError("Saved Successfully", EventLogEntryType.Information);
                    this.Close();
            }
            else
            {
                MessageBox.Show("Saving Failed,something go wrong,may be a " +
                   "system error try again later!",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Saving Failed", EventLogEntryType.Error);
            }

            }
          else 
                this.Close();
        }
    }
}
